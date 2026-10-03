using System.ComponentModel.DataAnnotations;

namespace Tour_Module.ViewModels.Location
{
    public class EditLocationViewModel
    {
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Location name is required.")]
        [StringLength(200, ErrorMessage = "Location name cannot exceed 200 characters.")]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; } = string.Empty;
    }
}