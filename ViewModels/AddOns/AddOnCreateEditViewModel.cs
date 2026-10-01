using System.ComponentModel.DataAnnotations;

namespace Tour_Module.ViewModels.AddOns
{
    public class AddOnCreateEditViewModel
    {
        public int? OptionId { get; set; }

        [Required(ErrorMessage ="Add-On name is required")]
        [StringLength(100, ErrorMessage ="Add-On name can not exceed 100 characters")]
        public string OptionName { get; set; } = string.Empty;

        [Required(ErrorMessage ="Base price is required")]
        [Range(1,double.MaxValue, ErrorMessage ="Base price must be greater than zero")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage ="Currency is required")]
        public string Currency { get; set; } = "INR";

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        // show during edit
        public bool IsActive { get; set; } = true;
    }
}
