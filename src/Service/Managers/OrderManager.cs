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
        private readonly IMapper _mapper;

        public OrderManager(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Order>();
            _mapper = mapper;
        }

        public OrderDto CreateOrder(CreateOrderDto order)
        {
            if (order.Quantity <= 0)
                throw new Exception("Quantity is invalid.");

            var entity = _mapper.Map<Order>(order);
            _repository.Create(entity);
            return _mapper.Map<OrderDto>(entity);
        }
    }
}
