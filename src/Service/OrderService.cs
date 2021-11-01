using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Order;
using Service.Contracts;
using System;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _repository;
        private readonly IProductService _productService;
        private readonly ITimeManager _systemConfigService;
        private readonly IMapper _mapper;

        public OrderService(IRepositoryFactory repositoryFactory, 
            IProductService productService, 
            ITimeManager systemConfigService, 
            IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Order>();
            _productService = productService;
            _systemConfigService = systemConfigService;
            _mapper = mapper;
        }

        public OrderDto CreateOrder(CreateOrderDto order)
        {
            if (order.Quantity <= 0)
                throw new Exception("Quantity is invalid.");

            var product = _productService.GetProductInfo(order.ProductCode);
            var newStock = product.Stock - order.Quantity;
            if (newStock < 0)
                throw new Exception("There is not enough stock.");

            var entity = _mapper.Map<Order>(order);
            entity.ProductId = product.Id;
            entity.CreationTime = _systemConfigService.GetTimeValue();

            _repository.Create(entity);

            _productService.UpdateProductStock(product.Id, newStock);

            return _mapper.Map<OrderDto>(entity);
        }
    }
}
