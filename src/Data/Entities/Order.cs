using System;

namespace Data.Entities
{
    public class Order : EntityBase
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        public Product Product { get; set; }

        public Order()
        {

        }
    }
}
