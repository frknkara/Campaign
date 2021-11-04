using Model.Campaign;
using Model.Order;
using System.Collections.Generic;

namespace Service.Contracts
{
    public interface ICampaignManager
    {
        CampaignDto CreateCampaign(CreateCampaignDto campaign);
        CampaignDto GetCampaignInfo(string campaignName);
        List<OrderDto> GetCampaignOrders(CampaignDto campaign);
        List<CampaignDto> GetActiveCampaigns(int time);
    }
}
