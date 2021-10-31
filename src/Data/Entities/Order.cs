using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Order : EntityBase
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }

        public Order()
        {

        }
    }
}
