using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Data.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        [Fact]
        public void Test_Should_Create_Order()
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

            var order = new Order
            {
                Quantity = 50
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                order.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Order>();
                repository.Create(order);

                var result = context.Orders.AsNoTracking().Include(x => x.Product).First(x => x.Id == order.Id);
                Assert.NotNull(result);
                Assert.Equal(order.Quantity, result.Quantity);
                Assert.Equal(product.Id, result.ProductId);
                Assert.NotNull(result.Product);
            }
        }

        [Fact]
        public void Test_Should_GetList_Order()
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

            var order1 = new Order
            {
                Quantity = 10
            };

            var order2 = new Order
            {
                Quantity = 20
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                order1.ProductId = product.Id;
                order2.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Order>();
                repository.Create(order1);
                repository.Create(order2);

                var result = repository.GetAll().ToList();
                Assert.NotNull(result);
                result = result.Where(x => x.ProductId == product.Id).ToList();
                Assert.True(result.Count == 2);
            }
        }

        [Fact]
        public void Test_Should_Get_Order()
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

            var order = new Order
            {
                Quantity = 20
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                order.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Order>();
                repository.Create(order);

                var result = repository.Get(order.Id);
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.ProductId);
                Assert.Equal(order.Quantity, result.Quantity);
            }
        }

        [Fact]
        public void Test_Should_GetByCondition_Order()
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

            var order = new Order
            {
                Quantity = 20
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                order.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Order>();
                repository.Create(order);

                var result = repository.GetByCondition(x => x.ProductId == product.Id).First();
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.ProductId);
                Assert.Equal(order.Quantity, result.Quantity);
            }
        }

        [Fact]
        public void Test_Should_Update_Order()
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

            var order = new Order
            {
                Quantity = 20
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                order.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Order>();
                repository.Create(order);

                order.Quantity++;
                repository.Update(order);

                var result = repository.Get(order.Id);
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.ProductId);
                Assert.Equal(order.Quantity, result.Quantity);
            }
        }

        [Fact]
        public void Test_Should_Delete_Order()
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

            var order = new Order
            {
                Quantity = 20
            };

            using (var context = new CampaignDbContext(options))
            {
                var repositoryFactory = new GenericRepositoryFactory(context);
                var productRepository = repositoryFactory.GetRepository<Product>();
                productRepository.Create(product, false);
                order.ProductId = product.Id;

                repositoryFactory = new GenericRepositoryFactory(context);
                var repository = repositoryFactory.GetRepository<Order>();
                repository.Create(order);

                repository.Delete(order);

                var result = repository.Get(order.Id);
                Assert.Null(result);
            }
        }
    }
}
