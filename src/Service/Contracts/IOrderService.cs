using Model.Order;

namespace Service.Contracts
{
    public interface IOrderService
    {
        OrderDto CreateOrder(CreateOrderDto order);
    }
}
