using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Service;
using Service.Contracts;
using Service.Managers;
using AutoMapper;
using System;
using Service.Services;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    public class Program
    {
        private readonly CommandHandler _commandHandler;

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connStr = Environment.GetEnvironmentVariable("CAMPAIGN_DB_CONNECTION");
            if (string.IsNullOrEmpty(connStr))
                connStr = config.GetConnectionString("CampaignDbConnection");

            var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(new MappingProfiles()));
            var mapper = mapperConfiguration.CreateMapper();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    var env = context.HostingEnvironment;
                    var config = context.Configuration.GetSection("Logging");
                    logging.AddConfiguration(config);
                    logging.AddConsole();
                    logging.ClearProviders();
                })
                .ConfigureServices(services =>
                {
                    services.AddTransient<Program>();
                    services.AddDbContext<CampaignDbContext>(options => options.UseNpgsql(connStr));
                    services.AddSingleton(mapper);
                    services.AddScoped<IRepositoryFactory, GenericRepositoryFactory>();
                    services.AddTransient<ITimeManager, TimeManager>();
                    services.AddTransient<IProductManager, ProductManager>();
                    services.AddTransient<IOrderManager, OrderManager>();
                    services.AddTransient<ICampaignManager, CampaignManager>();
                    services.AddTransient<ITimeService, TimeService>();
                    services.AddTransient<IProductService, ProductService>();
                    services.AddTransient<IOrderService, OrderService>();
                    services.AddTransient<ICampaignService, CampaignService>();
                    services.AddSingleton<CommandHandler>();
                })
                .Build();

            var dbContext = host.Services.CreateScope().ServiceProvider.GetRequiredService<CampaignDbContext>();
            DataSeeding.Seed(dbContext);

            host.Services.GetRequiredService<Program>().Run(dbContext);
        }

        public Program(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Run(CampaignDbContext dbContext)
        {
            while (true)
            {
                Console.WriteLine("Enter command:");
                var command = Console.ReadLine();
                if (command.ToLowerInvariant() == "exit" || command.ToLowerInvariant() == "quit")
                    break;
                if (command.ToLowerInvariant() == "reset_system_data")
                {
                    ConsoleKeyInfo response;
                    do
                    {
                        Console.Write("Are you sure to reset system data? [y/n] ");
                        response = Console.ReadKey();
                        Console.WriteLine();
                        if (response.Key == ConsoleKey.Y)
                        {
                            DataSeeding.ResetDb(dbContext);
                            Console.WriteLine("Done reset.");
                            break;
                        }
                    }
                    while (response.Key != ConsoleKey.N);
                    continue;
                }
                try
                {
                    var result = _commandHandler.RunCommand(command);
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine();
            }
        }
    }
}
