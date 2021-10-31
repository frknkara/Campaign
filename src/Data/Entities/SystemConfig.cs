using Model.Shared;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class SystemConfig : EntityBase
    {
        [Required]
        [StringLength(Constraints.CODE_COLUMN_MAX_LENGTH)]
        public string Code { get; set; }
        [Required]
        [StringLength(Constraints.TEXT_COLUMN_MAX_LENGTH)]
        public string Value { get; set; }

        public SystemConfig()
        {

        }
    }
}
