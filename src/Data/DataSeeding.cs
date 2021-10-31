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
                context.SystemConfigs.Add(new SystemConfig
                {
                    Id = Guid.NewGuid(),
                    Code = Constraints.TIME_SYSTEM_CONFIG_KEY,
                    Value = "0",
                    CreationTime = 0
                });
                context.SaveChanges();
            }
        }
    }
}
