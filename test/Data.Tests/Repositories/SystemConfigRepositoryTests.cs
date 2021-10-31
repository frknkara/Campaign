using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Data.Tests.Repositories
{
    public class SystemConfigRepositoryTests
    {
        [Fact]
        public void Test_Should_Create_SystemConfig()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var systemConfig = new SystemConfig
            {
                Code = "systemConfig_create",
                Value = "sample"
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<SystemConfig>();
                repository.Create(systemConfig);

                var result = context.SystemConfigs.AsNoTracking().First(x => x.Code == systemConfig.Code);
                Assert.NotNull(result);
                Assert.NotEqual(result.Id, Guid.Empty);
                Assert.Equal(systemConfig.Code, result.Code);
                Assert.Equal(systemConfig.Value, result.Value);
            }
        }

        [Fact]
        public void Test_Should_GetList_SystemConfig()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var systemConfig1 = new SystemConfig
            {
                Code = "systemConfig_getlist_1",
                Value = "sample1"
            };

            var systemConfig2 = new SystemConfig
            {
                Code = "systemConfig_getlist_2",
                Value = "sample2"
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<SystemConfig>();
                repository.Create(systemConfig1);
                repository.Create(systemConfig2);

                var result = repository.GetAll().ToList();
                Assert.NotNull(result);
                Assert.True(result.Count >= 2);
                result = result.Where(x => x.Code == systemConfig1.Code || x.Code == systemConfig2.Code).ToList();
                Assert.Equal(systemConfig1.Code, result[0].Code);
                Assert.Equal(systemConfig1.Value, result[0].Value);
                Assert.Equal(systemConfig2.Code, result[1].Code);
                Assert.Equal(systemConfig2.Value, result[1].Value);
            }
        }

        [Fact]
        public void Test_Should_Get_SystemConfig()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var systemConfig = new SystemConfig
            {
                Code = "systemConfig_get",
                Value = "sample"
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<SystemConfig>();
                repository.Create(systemConfig);

                var result = repository.Get(systemConfig.Id);
                Assert.NotNull(result);
                Assert.Equal(systemConfig.Code, result.Code);
                Assert.Equal(systemConfig.Value, result.Value);
            }
        }

        [Fact]
        public void Test_Should_GetByCondition_SystemConfig()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var systemConfig = new SystemConfig
            {
                Code = "systemConfig_getbycondition",
                Value = "sample"
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<SystemConfig>();
                repository.Create(systemConfig);

                var result = repository.GetByCondition(x => x.Code == systemConfig.Code).First();
                Assert.NotNull(result);
                Assert.Equal(systemConfig.Code, result.Code);
                Assert.Equal(systemConfig.Value, result.Value);
            }
        }

        [Fact]
        public void Test_Should_Update_SystemConfig()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var systemConfig = new SystemConfig
            {
                Code = "systemConfig_update",
                Value = "ABC"
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<SystemConfig>();
                repository.Create(systemConfig);

                systemConfig.Value = systemConfig.Value + "D";
                repository.Update(systemConfig);

                var result = repository.Get(systemConfig.Id);
                Assert.NotNull(result);
                Assert.Equal(systemConfig.Code, result.Code);
                Assert.Equal(systemConfig.Value, result.Value);
            }
        }

        [Fact]
        public void Test_Should_Delete_SystemConfig()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var systemConfig = new SystemConfig
            {
                Code = "systemConfig_delete",
                Value = "sample"
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<SystemConfig>();
                repository.Create(systemConfig);

                repository.Delete(systemConfig);

                var result = repository.Get(systemConfig.Id);
                Assert.Null(result);
            }
        }
    }
}
