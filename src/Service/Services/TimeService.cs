using Model.Campaign;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class TimeService : ITimeService
    {
        private readonly ITimeManager _timeManager;
        private readonly ICampaignManager _campaignManager;
        private readonly IProductManager _productManager;

        public TimeService(ITimeManager timeManager, ICampaignManager campaignManager, IProductManager productManager)
        {
            _timeManager = timeManager;
            _campaignManager = campaignManager;
            _productManager = productManager;
        }

        public string IncreaseTime(int hour)
        {
            if (hour <= 0)
                throw new Exception("Time value is not valid.");

            var oldTime = _timeManager.GetTimeValue();
            List<CampaignDto> oldActiveCampaigns = _campaignManager.GetActiveCampaigns(oldTime);

            var updatedTime = _timeManager.IncreaseTimeValue(hour);
            List<CampaignDto> currentActiveCampaigns = _campaignManager.GetActiveCampaigns(updatedTime);

            SetNewPrices(hour, currentActiveCampaigns);

            var closedCampaigns = GetClosedCampaigns(oldActiveCampaigns, currentActiveCampaigns);
            SetPricesToInitials(closedCampaigns);
            var time = updatedTime % 24;
            return time.ToString("D2") + ":00";
        }

        private void SetNewPrices(int incrementHour, List<CampaignDto> campaigns)
        {
            campaigns.ForEach(x => _productManager.UpdateProductPrice(
                x.ProductId,
                GetNewPrice(incrementHour, x.ProductPrice, x.ProductInitialPrice, x.PriceManipulationLimit, x.Duration)));
        }

        private void SetPricesToInitials(List<CampaignDto> campaigns)
        {
            campaigns.ForEach(x => _productManager.UpdateProductPrice(x.ProductId, x.ProductInitialPrice));
        }

        private List<CampaignDto> GetClosedCampaigns(List<CampaignDto> oldActiveCampaigns, List<CampaignDto> currentActiveCampaigns)
        {
            return oldActiveCampaigns.Where(p => !currentActiveCampaigns.Any(l => p.Id == l.Id)).ToList();
        }

        private int GetDiscountAmount(int initialPrice, int maxLimit, int duration)
        {
            return (int)Math.Ceiling(initialPrice * ((double)maxLimit / 100) / duration);
        }

        private double GetMinPrice(int initialPrice, int maxLimit)
        {
            return initialPrice - (initialPrice * (double)maxLimit / 100);
        }

        private int GetNewPrice(int incrementHour, int currentPrice, int initialPrice, int maxLimit, int duration)
        {
            var discountAmount = incrementHour * GetDiscountAmount(initialPrice, maxLimit, duration);
            var minPrice = GetMinPrice(initialPrice, maxLimit);
            if (currentPrice - discountAmount < minPrice)
                return (int)Math.Ceiling(minPrice);
            return currentPrice - discountAmount;
        }
    }
}
