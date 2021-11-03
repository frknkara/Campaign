using Model.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Product : EntityBase
    {
        [Required]
        [StringLength(Constraints.CODE_COLUMN_MAX_LENGTH)]
        public string Code { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int InitialPrice { get; set; }

        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Product()
        {

        }
    }
}
