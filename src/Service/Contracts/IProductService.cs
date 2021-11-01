using Model.Product;
using System;

namespace Service.Contracts
{
    public interface IProductService
    {
        ProductDto GetProductInfo(string productCode);
        ProductDto CreateProduct(CreateProductDto product);
        void UpdateProductStock(Guid id, int stock);
        void UpdateProductPrice(Guid id, int price);
    }
}
