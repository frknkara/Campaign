using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Order;
using Model.Product;
using Moq;
using Service;
using Service.Contracts;
using Service.Managers;
using System;
using Xunit;

namespace ServiceTests
{
    public class OrderManagerTests
    {
        private Mock<IRepositoryFactory> _mockRepositoryFactory;
        private Mock<IProductManager> _mockProductService;
        private Mock<ITimeManager> _mockTimeManager;
        private IMapper _mapper;
        private Guid sampleProductId;
        private ProductDto sampleProduct;

        public OrderManagerTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();

            _mockProductService = new Mock<IProductManager>();
            sampleProductId = Guid.NewGuid();
            sampleProduct = new ProductDto
            {
                Id = sampleProductId,
                Code = "product",
                Price = 100,
                Stock = 50,
                CreationTime = 0
            };
            _mockProductService.Setup(x => x.GetProductInfo(It.IsAny<string>())).Returns(sampleProduct);

            _mockTimeManager = new Mock<ITimeManager>();
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);

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
            var service = new OrderManager(_mockRepositoryFactory.Object, _mockProductService.Object, _mockTimeManager.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => service.CreateOrder(createOrder));
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
            var service = new OrderManager(_mockRepositoryFactory.Object, _mockProductService.Object, _mockTimeManager.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => service.CreateOrder(createOrder));
            Assert.Equal("Quantity is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateOrder_Insufficient_Quantity()
        {
            var createOrder = new CreateOrderDto
            {
                ProductCode = "product",
                Quantity = sampleProduct.Stock + 1
            };
            var service = new OrderManager(_mockRepositoryFactory.Object, _mockProductService.Object, _mockTimeManager.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => service.CreateOrder(createOrder));
            _mockProductService.Verify(x => x.GetProductInfo(sampleProduct.Code));
            Assert.Equal("There is not enough stock.", exception.Message);
        }

        [Fact]
        public void Test_CreateOrder()
        {
            var createOrder = new CreateOrderDto
            {
                ProductCode = "product",
                Quantity = 1
            };
            var _mockRepository = new Mock<IRepository<Order>>();
            _mockRepositoryFactory.Setup(x => x.GetRepository<Order>()).Returns(_mockRepository.Object);
            var service = new OrderManager(_mockRepositoryFactory.Object, _mockProductService.Object, _mockTimeManager.Object, _mapper);
            var orderResult = service.CreateOrder(createOrder);
            _mockProductService.Verify(x => x.GetProductInfo(sampleProduct.Code));
            _mockTimeManager.Verify(x => x.GetTimeValue());
            
            _mockRepository.Verify(x => x.Create(
                It.IsAny<Order>(),
                It.IsAny<bool>()));
            _mockProductService.Verify(x => x.UpdateProductStock(sampleProductId, sampleProduct.Stock - createOrder.Quantity));
            Assert.Equal(sampleProductId, orderResult.ProductId);
            Assert.Equal(createOrder.Quantity, orderResult.Quantity);
        }
    }
}
