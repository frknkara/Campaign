using System;

namespace Model.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int CreationTime { get; set; }
        public int UnitPrice { get; set; }
    }
}
