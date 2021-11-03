using Model.Campaign;
using Service.Contracts;
using System;
using System.Linq;

namespace Service.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignManager _campaignManager;
        private readonly IProductManager _productManager;
        private readonly ITimeManager _timeManager;

        public CampaignService(ICampaignManager campaignManager, IProductManager productManager, ITimeManager timeManager)
        {
            _campaignManager = campaignManager;
            _productManager = productManager;
            _timeManager = timeManager;
        }

        public string CreateCampaign(string name, string productCode, int duration, int priceManipulationLimit, int targetSalesCount)
        {
            var product = _productManager.GetProductInfo(productCode);
            var currentTime = _timeManager.GetTimeValue();
            var activeCampaign = _productManager.GetActiveCampaign(product, currentTime);
            if (activeCampaign != null)
                throw new Exception($"There is an active campaign with {activeCampaign.Name} name. New campaign can't be created for this product.");
            var campaign = new CreateCampaignDto
            {
                Name = name,
                ProductId = product.Id,
                ProductCode = productCode,
                Duration = duration,
                PriceManipulationLimit = priceManipulationLimit,
                TargetSalesCount = targetSalesCount,
                CreationTime = currentTime
            };
            var result = _campaignManager.CreateCampaign(campaign);
            return $"Campaign created; name {result.Name}, product {productCode}, duration {result.Duration}, limit {result.PriceManipulationLimit}, target sales count {result.TargetSalesCount}";
        }

        public string GetCampaignInfo(string name)
        {
            var campaign = _campaignManager.GetCampaignInfo(name);
            var currentTime = _timeManager.GetTimeValue();
            var status = campaign.CreationTime + campaign.Duration < currentTime ? "Ended" : "Active";
            var orders = _campaignManager.GetCampaignOrders(campaign);
            var totalSales = orders.Sum(x => x.Quantity);
            var turnover = orders.Sum(x => x.UnitPrice * x.Quantity);
            var averageItemPrice = GetFormattedAverageItemPrice(turnover, totalSales);
            return $"Campaign {campaign.Name} info; Status {status}, Target Sales {campaign.TargetSalesCount}, Total Sales {totalSales}, Turnover {turnover}, Average Item Price {averageItemPrice}";
        }

        private string GetFormattedAverageItemPrice(int turnover, int totalSales)
        {
            return (totalSales == 0) ? "-" : string.Format("{0:0.##}", (double)turnover / totalSales).Replace(",", ".");
        }
    }
}
