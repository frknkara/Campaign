using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Product;
using Model.Shared;
using Service.Contracts;
using System;
using System.Linq;

namespace Service.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IRepository<Product> _repository;
        private readonly ITimeManager _timeManager;
        private readonly IMapper _mapper;

        public ProductManager(IRepositoryFactory repositoryFactory, ITimeManager timeManager, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Product>();
            _timeManager = timeManager;
            _mapper = mapper;
        }

        public ProductDto CreateProduct(CreateProductDto product)
        {
            if (string.IsNullOrWhiteSpace(product.Code))
                throw new Exception("Product code is not valid.");
            if (product.Code.Length > Constraints.CODE_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of product code should be {Constraints.CODE_COLUMN_MAX_LENGTH}.");
            if (product.Price <= 0)
                throw new Exception("Price is invalid.");
            if (product.Stock <= 0)
                throw new Exception("Stock is invalid.");
            var existingProduct = _repository.GetByCondition(x => x.Code == product.Code).FirstOrDefault();
            if (existingProduct != null)
                throw new Exception($"The product with {product.Code} code has already been created.");

            var entity = _mapper.Map<Product>(product);
            entity.CreationTime = _timeManager.GetTimeValue();

            _repository.Create(entity);
            return _mapper.Map<ProductDto>(entity);
        }

        public ProductDto GetProductInfo(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
                throw new Exception("Product code is not valid.");
            if (productCode.Length > Constraints.CODE_COLUMN_MAX_LENGTH)
                throw new Exception($"Max length of product code should be {Constraints.CODE_COLUMN_MAX_LENGTH}.");

            var product = _repository.GetByCondition(x => x.Code == productCode).FirstOrDefault();
            if (product == null)
                throw new Exception("Product not found.");
            return _mapper.Map<ProductDto>(product);
        }

        public void UpdateProductPrice(Guid id, int price)
        {
            if (price <= 0)
                throw new Exception("Price is invalid.");
            var product = _repository.Get(id);
            if (product == null)
                throw new Exception("Product not found.");
            product.Price = price;
            _repository.Update(product);
        }

        public void UpdateProductStock(Guid id, int stock)
        {
            if (stock < 0)
                throw new Exception("Stock is invalid.");
            var product = _repository.Get(id);
            if (product == null)
                throw new Exception("Product not found.");
            product.Stock = stock;
            _repository.Update(product);
        }
    }
}
