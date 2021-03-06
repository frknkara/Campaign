using System;

namespace Model.Campaign
{
    public class CreateCampaignDto
    {
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public int PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
        public int CreationTime { get; set; }
    }
}
