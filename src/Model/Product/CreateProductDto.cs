namespace Model.Product
{
    public class CreateProductDto
    {
        public string Code { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public int CreationTime { get; set; }
    }
}
