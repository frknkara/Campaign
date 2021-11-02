using Model.Product;
using Service.Contracts;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductManager _productManager;
        private readonly ITimeManager _timeManager;

        public ProductService(IProductManager productManager, ITimeManager timeManager)
        {
            _productManager = productManager;
            _timeManager = timeManager;
        }

        public string CreateProduct(string code, int price, int stock)
        {
            var product = new CreateProductDto
            {
                Code = code,
                Price = price,
                Stock = stock,
                CreationTime = _timeManager.GetTimeValue()
            };
            var result = _productManager.CreateProduct(product);
            return $"Product created; code {result.Code}, price {result.Price}, stock {result.Stock}";
        }

        public string GetProductInfo(string code)
        {
            var result = _productManager.GetProductInfo(code);
            return $"Product {result.Code} info; price {result.Price}, stock {result.Stock}";
        }
    }
}
