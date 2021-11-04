using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Model.Shared;
using System;

namespace Data
{
    public static class DataSeeding
    {
        public static void Seed(CampaignDbContext context)
        {
            if (context.Database.EnsureCreated())
            {
                context.Database.Migrate();
                AddInitialTime(context);
                context.SaveChanges();
            }
        }

        public static void ResetDb(CampaignDbContext context)
        {
            context.Campaigns.RemoveRange(context.Campaigns);
            context.Orders.RemoveRange(context.Orders);
            context.Products.RemoveRange(context.Products);
            context.SystemConfigs.RemoveRange(context.SystemConfigs);
            AddInitialTime(context);
            context.SaveChanges();
        }

        private static void AddInitialTime(CampaignDbContext context)
        {
            context.SystemConfigs.Add(new SystemConfig
            {
                Id = Guid.NewGuid(),
                Code = Constraints.TIME_SYSTEM_CONFIG_KEY,
                Value = "0",
                CreationTime = 0
            });
        }
    }
}
