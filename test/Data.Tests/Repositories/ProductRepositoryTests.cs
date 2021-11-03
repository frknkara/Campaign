using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Data.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void Test_Should_Create_Product()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_create",
                Price = 10,
                Stock = 100,
                InitialPrice = 10
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);
                Assert.Equal(DateTime.Today, product.RealCreationTime.Date);

                var result = context.Products.AsNoTracking().First(x => x.Code == product.Code);
                Assert.NotNull(result);
                Assert.NotEqual(result.Id, Guid.Empty);
                Assert.Equal(product.Code, result.Code);
                Assert.Equal(product.Price, result.Price);
                Assert.Equal(product.Stock, result.Stock);
                Assert.Equal(product.InitialPrice, result.InitialPrice);
                Assert.Equal(product.RealCreationTime, result.RealCreationTime);
            }
        }

        [Fact]
        public void Test_Should_Create_Product_But_Not_Save_Db()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_create_not_save",
                Price = 10,
                Stock = 100
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product, false);
            }

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();

                var result = context.Products.AsNoTracking().FirstOrDefault(x => x.Code == product.Code);
                Assert.Null(result);
            }
        }

        [Fact]
        public void Test_Should_GetList_Product()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product1 = new Product
            {
                Code = "product_getlist_1",
                Price = 10,
                Stock = 100,
                InitialPrice = 10
            };

            var product2 = new Product
            {
                Code = "product_getlist_2",
                Price = 20,
                Stock = 500,
                InitialPrice = 30
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product1);
                repository.Create(product2);

                var result = repository.GetAll().ToList();
                Assert.NotNull(result);
                Assert.True(result.Count >= 2);
                result = result.Where(x => x.Code == product1.Code || x.Code == product2.Code).ToList();
                Assert.Equal(product1.Code, result[0].Code);
                Assert.Equal(product1.Price, result[0].Price);
                Assert.Equal(product1.Stock, result[0].Stock);
                Assert.Equal(product1.InitialPrice, result[0].InitialPrice);
                Assert.Equal(product2.Code, result[1].Code);
                Assert.Equal(product2.Price, result[1].Price);
                Assert.Equal(product2.Stock, result[1].Stock);
                Assert.Equal(product2.InitialPrice, result[1].InitialPrice);
            }
        }

        [Fact]
        public void Test_Should_Get_Product()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_get",
                Price = 10,
                Stock = 100,
                InitialPrice = 5
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);

                var result = repository.Get(product.Id);
                Assert.NotNull(result);
                Assert.Equal(product.Code, result.Code);
                Assert.Equal(product.Price, result.Price);
                Assert.Equal(product.Stock, result.Stock);
                Assert.Equal(product.InitialPrice, result.InitialPrice);
            }
        }

        [Fact]
        public void Test_Should_GetByCondition_Product()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_getbycondition",
                Price = 10,
                Stock = 100,
                InitialPrice = 5
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);

                var result = repository.GetByCondition(x => x.Code == product.Code).First();
                Assert.NotNull(result);
                Assert.Equal(product.Code, result.Code);
                Assert.Equal(product.Price, result.Price);
                Assert.Equal(product.Stock, result.Stock);
                Assert.Equal(product.InitialPrice, result.InitialPrice);
            }
        }

        [Fact]
        public void Test_Should_Update_Product()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_update",
                Price = 10,
                Stock = 100,
                InitialPrice = 5
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);

                product.Price++;
                repository.Update(product);

                var result = repository.Get(product.Id);
                Assert.NotNull(result);
                Assert.Equal(product.Code, result.Code);
                Assert.Equal(product.Price, result.Price);
                Assert.Equal(product.Stock, result.Stock);
                Assert.Equal(product.InitialPrice, result.InitialPrice);
            }
        }

        [Fact]
        public void Test_Should_Update_Product_But_Not_Save_Db()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_update_not_save",
                Price = 10,
                Stock = 100
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);

                product.Price++;
                repository.Update(product, false);
            }

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                var result = repository.Get(product.Id);
                Assert.Equal(product.Price, result.Price + 1);
            }
        }

        [Fact]
        public void Test_Should_Delete_Product()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_delete",
                Price = 10,
                Stock = 100
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);

                repository.Delete(product);

                var result = repository.Get(product.Id);
                Assert.Null(result);
            }
        }

        [Fact]
        public void Test_Should_Delete_Product_But_Not_Save_Db()
        {
            var options = new DbContextOptionsBuilder<CampaignDbContext>()
                .UseInMemoryDatabase("campaigndb")
                .Options;

            var product = new Product
            {
                Code = "product_delete_not_save",
                Price = 10,
                Stock = 100
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();
                repository.Create(product);

                repository.Delete(product, false);
            }

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Product>();

                var result = repository.Get(product.Id);
                Assert.NotNull(result);
            }
        }
    }
}
