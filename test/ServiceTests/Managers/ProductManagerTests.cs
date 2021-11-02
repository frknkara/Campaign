using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Product;
using Model.Shared;
using Moq;
using Service;
using Service.Contracts;
using Service.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace ServiceTests
{
    public class ProductManagerTests
    {
        private Mock<IRepositoryFactory> _mockRepositoryFactory;
        private IMapper _mapper;

        public ProductManagerTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();

            var mapperConfiguration = new MapperConfiguration(conf => conf.AddProfile(new MappingProfiles()));
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void Test_CreateProduct_Null_Code()
        {
            var createProduct = new CreateProductDto
            {
                Code = null,
                Stock = 100,
                Price = 20
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal("Product code is not valid.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Empty_String_Code()
        {
            var createProduct = new CreateProductDto
            {
                Code = "",
                Stock = 100,
                Price = 20
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal("Product code is not valid.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Exceeds_Max_Length_Code()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.CODE_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var createProduct = new CreateProductDto
            {
                Code = invalidLengthString,
                Stock = 100,
                Price = 20
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal($"Max length of product code should be {Constraints.CODE_COLUMN_MAX_LENGTH}.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Zero_Price()
        {
            var createProduct = new CreateProductDto
            {
                Code = "product",
                Stock = 100,
                Price = 0
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal("Price is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Negative_Price()
        {
            var createProduct = new CreateProductDto
            {
                Code = "product",
                Stock = 100,
                Price = -1
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal("Price is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Zero_Stock()
        {
            var createProduct = new CreateProductDto
            {
                Code = "product",
                Stock = 0,
                Price = 20
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal("Stock is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Negative_Stock()
        {
            var createProduct = new CreateProductDto
            {
                Code = "product",
                Stock = -1,
                Price = 20
            };
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal("Stock is invalid.", exception.Message);
        }

        [Fact]
        public void Test_CreateProduct_Same_Product_Code_Exist()
        {
            var createProduct = new CreateProductDto
            {
                Code = "product",
                Stock = 100,
                Price = 20
            };
            var _mockRepository = new Mock<IRepository<Product>>();
            var list = new List<Product>
            {
                new Product
                {
                    Code = "product",
                    Stock = 100,
                    Price = 50
                }
            };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Product, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.CreateProduct(createProduct));
            Assert.Equal($"The product with {createProduct.Code} code has already been created.", exception.Message);
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
            var _mockRepository = new Mock<IRepository<Product>>();
            var list = new List<Product>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Product, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            var productResult = manager.CreateProduct(createProduct);
            _mockRepository.Verify(x => x.Create(
                It.IsAny<Product>(),
                It.IsAny<bool>()));
            Assert.Equal(createProduct.Code, productResult.Code);
        }

        [Fact]
        public void Test_GetProductInfo_Null_Code()
        {
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetProductInfo(null));
            Assert.Equal("Product code is not valid.", exception.Message);
        }

        [Fact]
        public void Test_GetProductInfo_Empty_String_Code()
        {
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetProductInfo(""));
            Assert.Equal("Product code is not valid.", exception.Message);
        }

        [Fact]
        public void Test_GetProductInfo_Exceeds_Max_Length_Code()
        {
            var invalidLengthString = new string(Enumerable.Repeat("ABCDEF", Constraints.CODE_COLUMN_MAX_LENGTH + 1).Select(s => s[new Random().Next(s.Length)]).ToArray());
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetProductInfo(invalidLengthString));
            Assert.Equal($"Max length of product code should be {Constraints.CODE_COLUMN_MAX_LENGTH}.", exception.Message);
        }

        [Fact]
        public void Test_GetProductInfo_Product_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<Product>>();
            var list = new List<Product>();
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Product, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.GetProductInfo("product"));
            Assert.Equal("Product not found.", exception.Message);
        }

        [Fact]
        public void Test_GetProductInfo()
        {
            var _mockRepository = new Mock<IRepository<Product>>();
            var sampleProduct = new Product
            {
                Code = "product",
                Stock = 100,
                Price = 50
            };
            var list = new List<Product> { sampleProduct };
            _mockRepository.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<Product, bool>>>())).Returns(list.AsQueryable());
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            var product = manager.GetProductInfo("product");
            Assert.Equal(sampleProduct.Code, product.Code);
            Assert.Equal(sampleProduct.Stock, product.Stock);
            Assert.Equal(sampleProduct.Price, product.Price);
        }

        [Fact]
        public void Test_UpdateProductPrice_Zero_Price()
        {
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.UpdateProductPrice(Guid.NewGuid(), 0));
            Assert.Equal("Price is invalid.", exception.Message);
        }

        [Fact]
        public void Test_UpdateProductPrice_Negative_Price()
        {
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.UpdateProductPrice(Guid.NewGuid(), -1));
            Assert.Equal("Price is invalid.", exception.Message);
        }

        [Fact]
        public void Test_UpdateProductPrice_Product_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<Product>>();
            Product product = null;
            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(product);
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.UpdateProductPrice(Guid.NewGuid(), 10));
            Assert.Equal("Product not found.", exception.Message);
        }

        [Fact]
        public void Test_UpdateProductPrice()
        {
            var sampleGuid = Guid.NewGuid();
            var _mockRepository = new Mock<IRepository<Product>>();
            var product = new Product
            {
                Id = sampleGuid,
                Code = "product",
                Stock = 100,
                Price = 50
            }; 
            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(product);
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            manager.UpdateProductPrice(sampleGuid, 10);
            _mockRepository.Verify(x => x.Update(
                It.IsAny<Product>(),
                It.IsAny<bool>()));
            Assert.Equal(10, product.Price);
        }

        [Fact]
        public void Test_UpdateProductStock_Zero_Stock()
        {
            var sampleGuid = Guid.NewGuid();
            var _mockRepository = new Mock<IRepository<Product>>();
            var product = new Product
            {
                Id = sampleGuid,
                Code = "product",
                Stock = 100,
                Price = 50
            };
            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(product);
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            manager.UpdateProductStock(sampleGuid, 0);
            _mockRepository.Verify(x => x.Update(
                It.IsAny<Product>(),
                It.IsAny<bool>()));
            Assert.Equal(0, product.Stock);
        }

        [Fact]
        public void Test_UpdateProductStock_Negative_Stock()
        {
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.UpdateProductStock(Guid.NewGuid(), -1));
            Assert.Equal("Stock is invalid.", exception.Message);
        }

        [Fact]
        public void Test_UpdateProductStock_Product_Not_Found()
        {
            var _mockRepository = new Mock<IRepository<Product>>();
            Product product = null;
            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(product);
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            Exception exception = Assert.Throws<Exception>(() => manager.UpdateProductStock(Guid.NewGuid(), 10));
            Assert.Equal("Product not found.", exception.Message);
        }

        [Fact]
        public void Test_UpdateProductStock()
        {
            var sampleGuid = Guid.NewGuid();
            var _mockRepository = new Mock<IRepository<Product>>();
            var product = new Product
            {
                Id = sampleGuid,
                Code = "product",
                Stock = 100,
                Price = 50
            };
            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(product);
            _mockRepositoryFactory.Setup(x => x.GetRepository<Product>()).Returns(_mockRepository.Object);
            var manager = new ProductManager(_mockRepositoryFactory.Object, _mapper);
            manager.UpdateProductStock(sampleGuid, 10);
            _mockRepository.Verify(x => x.Update(
                It.IsAny<Product>(),
                It.IsAny<bool>()));
            Assert.Equal(10, product.Stock);
        }
    }
}
