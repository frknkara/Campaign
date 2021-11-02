using Model.Campaign;

namespace Service.Contracts
{
    public interface ICampaignManager
    {
        CampaignDto CreateCampaign(CreateCampaignDto campaign);
        CampaignDto GetCampaignInfo(string campaignName);
    }
}
