using Data;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using Service.Contracts;
using Service;
using AutoMapper;

namespace Campaign
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connStr = config.GetConnectionString("CampaignDbConnection");

            var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(new MappingProfiles()));
            var mapper = mapperConfiguration.CreateMapper();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<CampaignDbContext>(options => options.UseNpgsql(connStr));
                    services.AddSingleton(mapper);
                    services.AddScoped<IRepositoryFactory, GenericRepositoryFactory>();
                    services.AddTransient<ISystemConfigService, SystemConfigService>();
                })
                .Build();

            var dbContext = host.Services.CreateScope().ServiceProvider.GetRequiredService<CampaignDbContext>();
            DataSeeding.Seed(dbContext);

            var service = host.Services.GetRequiredService<ISystemConfigService>();
            //var list = repo.GetRepository<SystemConfig>().GetAll().ToList();
            service.IncreaseTimeValue();
            var time = service.GetTimeValue();
        }
    }
}
