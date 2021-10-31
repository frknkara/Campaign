using Model.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Campaign : EntityBase
    {
        [Required]
        [StringLength(Constraints.NAME_COLUMN_MAX_LENGTH)]
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public int Duration { get; set; }
        public int PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }

        public Product Product { get; set; }

        public Campaign()
        {

        }
    }
}
