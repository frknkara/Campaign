namespace Service.Contracts
{
    public interface ICampaignService
    {
        string CreateCampaign(string name, string productCode, int duration, int priceManipulationLimit, int targetSalesCount);
        string GetCampaignInfo(string name);
    }
}
