using System.ComponentModel.DataAnnotations;

namespace Tour_Module.ViewModels.Options
{
    public class OptionCreateViewModel
    {
        [Required(ErrorMessage = "Option name is required")]
        [StringLength(150, ErrorMessage = "Option name cannot exceed 150 characters")]
        public string OptionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Base price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Base price must be greater than zero")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; } = "INR";

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;
    }
}