using Model.Product;

namespace Service.Contracts
{
    public interface IProductService
    {
        string CreateProduct(string code, int price, int stock);
        string GetProductInfo(string code);
    }
}
