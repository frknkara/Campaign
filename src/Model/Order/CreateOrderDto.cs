using System;

namespace Model.Order
{
    public class CreateOrderDto
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int CreationTime { get; set; }
    }
}
