using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Data.Tests.Repositories
{
    public class CampaignRepositoryTests
    {
        [Fact]
        public void Test_Should_Create_Campaign()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product",
                Price = 10,
                Stock = 100
            };

            var campaign = new Campaign
            {
                Duration = 10,
                Name = "campaign",
                PriceManipulationLimit = 5,
                TargetSalesCount = 50
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                campaign.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Campaign>();
                repository.Create(campaign);

                var result = context.Campaigns.AsNoTracking().Include(x => x.Product).First(x => x.Id == campaign.Id);
                Assert.NotNull(result);
                Assert.Equal(campaign.Duration, result.Duration);
                Assert.Equal(campaign.Name, result.Name);
                Assert.Equal(campaign.PriceManipulationLimit, result.PriceManipulationLimit);
                Assert.Equal(campaign.TargetSalesCount, result.TargetSalesCount);
                Assert.Equal(product.Id, result.ProductId);
                Assert.NotNull(result.Product);
            }
        }

        [Fact]
        public void Test_Should_GetList_Campaign()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product",
                Price = 10,
                Stock = 100
            };

            var campaign1 = new Campaign
            {
                Duration = 10,
                Name = "campaign1",
                PriceManipulationLimit = 5,
                TargetSalesCount = 50
            };

            var campaign2 = new Campaign
            {
                Duration = 20,
                Name = "campaign2",
                PriceManipulationLimit = 10,
                TargetSalesCount = 70
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                campaign1.ProductId = product.Id;
                campaign2.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Campaign>();
                repository.Create(campaign1);
                repository.Create(campaign2);

                var result = repository.GetAll().ToList();
                Assert.NotNull(result);
                result = result.Where(x => x.ProductId == product.Id).ToList();
                Assert.True(result.Count == 2);
            }
        }

        [Fact]
        public void Test_Should_Get_Campaign()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product",
                Price = 10,
                Stock = 100
            };

            var campaign = new Campaign
            {
                Duration = 10,
                Name = "campaign",
                PriceManipulationLimit = 5,
                TargetSalesCount = 50
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                campaign.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Campaign>();
                repository.Create(campaign);

                var result = repository.Get(campaign.Id);
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.ProductId);
                Assert.Equal(campaign.Duration, result.Duration);
                Assert.Equal(campaign.Name, result.Name);
                Assert.Equal(campaign.PriceManipulationLimit, result.PriceManipulationLimit);
                Assert.Equal(campaign.TargetSalesCount, result.TargetSalesCount);
            }
        }

        [Fact]
        public void Test_Should_GetByCondition_Campaign()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product",
                Price = 10,
                Stock = 100
            };

            var campaign = new Campaign
            {
                Duration = 10,
                Name = "campaign",
                PriceManipulationLimit = 5,
                TargetSalesCount = 50
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                campaign.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Campaign>();
                repository.Create(campaign);

                var result = repository.GetByCondition(x => x.ProductId == product.Id).First();
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.ProductId);
                Assert.Equal(campaign.Duration, result.Duration);
                Assert.Equal(campaign.Name, result.Name);
                Assert.Equal(campaign.PriceManipulationLimit, result.PriceManipulationLimit);
                Assert.Equal(campaign.TargetSalesCount, result.TargetSalesCount);
            }
        }

        [Fact]
        public void Test_Should_Update_Campaign()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product",
                Price = 10,
                Stock = 100
            };

            var campaign = new Campaign
            {
                Duration = 10,
                Name = "campaign",
                PriceManipulationLimit = 5,
                TargetSalesCount = 50
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                campaign.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Campaign>();
                repository.Create(campaign);

                campaign.TargetSalesCount++;
                campaign.PriceManipulationLimit--;
                repository.Update(campaign);

                var result = repository.Get(campaign.Id);
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.ProductId);
                Assert.Equal(campaign.Duration, result.Duration);
                Assert.Equal(campaign.Name, result.Name);
                Assert.Equal(campaign.PriceManipulationLimit, result.PriceManipulationLimit);
                Assert.Equal(campaign.TargetSalesCount, result.TargetSalesCount);
            }
        }

        [Fact]
        public void Test_Should_Delete_Campaign()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product",
                Price = 10,
                Stock = 100
            };

            var campaign = new Campaign
            {
                Duration = 10,
                Name = "campaign",
                PriceManipulationLimit = 5,
                TargetSalesCount = 50
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                campaign.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Campaign>();
                repository.Create(campaign);

                repository.Delete(campaign);

                var result = repository.Get(campaign.Id);
                Assert.Null(result);
            }
        }
    }
}
