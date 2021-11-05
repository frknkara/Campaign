using AutoMapper;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Service;
using Service.Contracts;
using Service.Managers;
using Service.Services;
using System;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            var connStr = Environment.GetEnvironmentVariable("CAMPAIGN_DB_CONNECTION");
            if (string.IsNullOrEmpty(connStr))
                connStr = Configuration.GetConnectionString("CampaignDbConnection");

            services.AddDbContext<CampaignDbContext>(options => options.UseNpgsql(connStr));

            var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(new MappingProfiles()));
            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton(mapper);

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
            services.AddTransient<CommandHandler>();
            services.AddTransient<ResetSystemDataHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
