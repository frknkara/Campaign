using Model.Product;
using Moq;
using Service.Contracts;
using Service.Services;
using Xunit;

namespace ServiceTests
{
    public class ProductServiceTests
    {
        private Mock<IProductManager> _mockProductManager;
        private Mock<ITimeManager> _mockTimeManager;

        public ProductServiceTests()
        {
            _mockProductManager = new Mock<IProductManager>();
            _mockTimeManager = new Mock<ITimeManager>();
            _mockTimeManager.Setup(x => x.GetTimeValue()).Returns(0);
        }

        [Fact]
        public void Test_CreateProduct()
        {
            var createProduct = new CreateProductDto
            {
                Code = "product",
                Stock = 100,
                Price = 20
            };
            var product = new ProductDto
            {
                Code = "product",
                Stock = 100,
                Price = 20
            };
            _mockProductManager.Setup(x => x.CreateProduct(It.IsAny<CreateProductDto>())).Returns(product);
            var service = new ProductService(_mockProductManager.Object, _mockTimeManager.Object);
            var result = service.CreateProduct(createProduct.Code, createProduct.Price, createProduct.Stock);
            _mockTimeManager.Verify(x => x.GetTimeValue());
            Assert.Equal($"Product created; code {createProduct.Code}, price {createProduct.Price}, stock {createProduct.Stock}", result);
        }

        [Fact]
        public void Test_GetProductInfo()
        {
            var product = new ProductDto
            {
                Code = "product",
                Stock = 100,
                Price = 20
            };
            _mockProductManager.Setup(x => x.GetProductInfo(It.IsAny<string>())).Returns(product);
            var service = new ProductService(_mockProductManager.Object, _mockTimeManager.Object);
            var result = service.GetProductInfo(product.Code);
            Assert.Equal($"Product {product.Code} info; price {product.Price}, stock {product.Stock}", result);
        }
    }
}
