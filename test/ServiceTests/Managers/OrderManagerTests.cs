using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Order;
using Moq;
using Service;
using Service.Managers;
using System;
using Xunit;

namespace ServiceTests
{
    public class OrderManagerTests
    {
        private Mock<IRepositoryFactory> _mockRepositoryFactory;
        private IMapper _mapper;

        public OrderManagerTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();
            var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(new MappingProfiles()));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void Test_CreateOrder_Zero_Quantity()
        {
            var createOrder = new CreateOrderDto
            {
                ProductCode = "product",
                Quantity = 0
            };
            var manager = new OrderManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateOrder(createOrder));
            Assert.Equal("Quantity is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateOrder_Negative_Quantity()
        {
            var createOrder = new CreateOrderDto
            {
                ProductCode = "product",
                Quantity = -1
            };
            var manager = new OrderManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateOrder(createOrder));
            Assert.Equal("Quantity is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateOrder()
        {
            var createOrder = new CreateOrderDto
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "product",
                Quantity = 1,
                UnitPrice = 50
            };
            var _mockRepository = new Mock<IRepository<Order>>();
            _mockRepositoryFactory.Setup(x => x.GetRepository<Order>()).Returns(_mockRepository.Object);
            var manager = new OrderManager(_mockRepositoryFactory.Object, _mapper);
            var orderResult = manager.CreateOrder(createOrder);
            
            _mockRepository.Verify(x => x.Create(
                It.IsAny<Order>(),
                It.IsAny<bool>()));
            Assert.Equal(createOrder.ProductId, orderResult.ProductId);
            Assert.Equal(createOrder.Quantity, orderResult.Quantity);
            Assert.Equal(createOrder.UnitPrice, orderResult.UnitPrice);
        }
    }
}
