using System;

namespace Model.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public int CreationTime { get; set; }
        public int InitialPrice { get; set; }
    }
}
