using Model.Order;
using Service.Contracts;
using System;

namespace Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderManager _orderManager;
        private readonly IProductManager _productManager;
        private readonly ITimeManager _timeManager;

        public OrderService(IOrderManager orderManager, IProductManager productManager, ITimeManager timeManager)
        {
            _orderManager = orderManager;
            _productManager = productManager;
            _timeManager = timeManager;
        }

        public string CreateOrder(string productCode, int quantity)
        {
            var product = _productManager.GetProductInfo(productCode);
            var newStock = product.Stock - quantity;
            if (newStock < 0)
                throw new Exception("There is not enough stock.");
            var order = new CreateOrderDto
            {
                ProductId = product.Id,
                ProductCode = productCode,
                Quantity = quantity,
                CreationTime = _timeManager.GetTimeValue()
            };
            var result = _orderManager.CreateOrder(order);
            _productManager.UpdateProductStock(product.Id, newStock);
            return $"Order created; product {productCode}, quantity {result.Quantity}";
        }
    }
}
