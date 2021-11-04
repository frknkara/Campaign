using System;

namespace Model.Campaign
{
    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public int PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
        public int CreationTime { get; set; }
        public DateTime RealCreationTime { get; set; }
        public int ProductPrice { get; set; }
        public int ProductInitialPrice { get; set; }
    }
}
