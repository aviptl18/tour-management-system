using System.ComponentModel.DataAnnotations;

namespace Tour_Module.DTOs.Tours
{
    public class CreateTourStep1Dto
    {
        public int DraftId { get; set; }

        [Required(ErrorMessage = "Tour title is required.")]
        [MaxLength(100, ErrorMessage = "Tour title cannot exceed 100 characters.")]
        public string TourTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tour description is required.")]
        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Overview { get; set; } = string.Empty;
    }
}
