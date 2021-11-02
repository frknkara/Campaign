using AutoMapper;
using Data.Entities;
using Data.Repositories;
using Model.Order;
using Service.Contracts;
using System;

namespace Service.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IRepository<Order> _repository;
        private readonly IProductManager _productManager;
        private readonly ITimeManager _timeManager;
        private readonly IMapper _mapper;

        public OrderManager(IRepositoryFactory repositoryFactory, 
            IProductManager productManager, 
            ITimeManager timeManager, 
            IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Order>();
            _productManager = productManager;
            _timeManager = timeManager;
            _mapper = mapper;
        }

        public OrderDto CreateOrder(CreateOrderDto order)
        {
            if (order.Quantity <= 0)
                throw new Exception("Quantity is invalid.");

            var product = _productManager.GetProductInfo(order.ProductCode);
            var newStock = product.Stock - order.Quantity;
            if (newStock < 0)
                throw new Exception("There is not enough stock.");

            var entity = _mapper.Map<Order>(order);
            entity.ProductId = product.Id;
            entity.CreationTime = _timeManager.GetTimeValue();

            _repository.Create(entity);

            _productManager.UpdateProductStock(product.Id, newStock);

            return _mapper.Map<OrderDto>(entity);
        }
    }
}
