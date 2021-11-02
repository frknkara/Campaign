using Model.Order;

namespace Service.Contracts
{
    public interface IOrderManager
    {
        OrderDto CreateOrder(CreateOrderDto order);
    }
}
