using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Module.Models.Entities
{
    [Table("OptionDefinition")]
    public class OptionDefinition
    {
        [Key]
        public int OptionId { get; set; }

        public string OptionName { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public string Currency { get; set; } = "INR";

        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
