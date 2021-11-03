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
            var campaign = new CreateCampaignDto
            {
                Name = name,
                ProductId = product.Id,
                ProductCode = productCode,
                Duration = duration,
                PriceManipulationLimit = priceManipulationLimit,
                TargetSalesCount = targetSalesCount,
                CreationTime = _timeManager.GetTimeValue()
            };
            var result = _campaignManager.CreateCampaign(campaign);
            return $"Campaign created; name {result.Name}, product {productCode}, duration {result.Duration}, limit {result.PriceManipulationLimit}, target sales count {result.TargetSalesCount}";
        }

        public string GetCampaignInfo(string name)
        {
            var result = _campaignManager.GetCampaignInfo(name);
            var currentTime = _timeManager.GetTimeValue();
            var status = result.CreationTime + result.Duration < currentTime ? "Ended" : "Active";
            var orders = _campaignManager.GetCampaignOrders(name);
            var totalSales = orders.Sum(x => x.Quantity);
            var turnover = 0; //TODO: consider turnover
            var averageItemPrice = (totalSales == 0) ? "-" : string.Format("{0:0.##}", (double)turnover / totalSales);
            return $"Campaign {result.Name} info; Status {status}, Target Sales {result.TargetSalesCount}, Total Sales {totalSales}, Turnover {turnover}, Average Item Price {averageItemPrice}";
        }
    }
}
