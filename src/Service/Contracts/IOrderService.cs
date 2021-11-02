namespace Service.Contracts
{
    public interface IOrderService
    {
        string CreateOrder(string productCode, int quantity);
    }
}
