using Model.Order;
using Model.Product;
using Moq;
using Service.Contracts;
using Service.Services;
using System;
using Xunit;

namespace ServiceTests
{
    public class OrderServiceTests
    {
        private Mock<IOrderManager> _mockOrderManager;
        private Mock<IProductManager> _mockProductManager;
        private Mock<ITimeManager> _mockTimeManager;
        private Guid sampleProductId;
        private ProductDto sampleProduct;

        public OrderServiceTests()
        {
            _mockOrderManager = new Mock<IOrderManager>();
            _mockProductManager = new Mock<IProductManager>();
            _mockTimeManager = new Mock<ITimeManager>();
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);

            sampleProductId = Guid.NewGuid();
            sampleProduct = new ProductDto
            {
                Id = sampleProductId,
                Code = "product",
                Price = 100,
                Stock = 50,
                CreationTime = 0,
                InitialPrice = 100
            };
            _mockProductManager.Setup(x => x.GetProductInfo(It.IsAny<string>())).Returns(sampleProduct);
        }

        [Fact]
        public void Test_CreateOrder_Insufficient_Quantity()
        {
            var createOrder = new CreateOrderDto
            {
                ProductCode = "product",
                Quantity = 51
            };
            var service = new OrderService(_mockOrderManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            Exception exception = Assert.Throws<Exception>(() => service.CreateOrder(createOrder.ProductCode, createOrder.Quantity));
            _mockProductManager.Verify(x => x.GetProductInfo(createOrder.ProductCode));
            Assert.Equal("There is not enough stock.", exception.Message);
        }

        [Fact]
        public void Test_CreateOrder()
        {
            var createOrder = new CreateOrderDto
            {
                ProductCode = "product",
                Quantity = 5
            };
            var order = new OrderDto
            {
                ProductId = sampleProductId,
                Quantity = 5
            };
            _mockOrderManager.Setup(x => x.CreateOrder(It.IsAny<CreateOrderDto>())).Returns(order);
            var service = new OrderService(_mockOrderManager.Object, _mockProductManager.Object, _mockTimeManager.Object);
            var result = service.CreateOrder(createOrder.ProductCode, createOrder.Quantity);
            _mockProductManager.Verify(x => x.GetProductInfo(createOrder.ProductCode));
            _mockTimeManager.Verify(x => x.GetTimeValue());
            _mockProductManager.Verify(x => x.UpdateProductStock(sampleProductId, sampleProduct.Stock - createOrder.Quantity));
            Assert.Equal($"Order created; product {createOrder.ProductCode}, quantity {createOrder.Quantity}", result);
        }
    }
}
