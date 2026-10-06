using System.ComponentModel.DataAnnotations;

namespace Tour_Module.DTOs.Tours
{
    public class CreateTourStep1Dto
    {
        public int DraftId { get; set; }

        [Required(ErrorMessage = "Tour title is required.")]
        [MaxLength(200, ErrorMessage = "Tour title cannot exceed 200 characters.")]
        public string TourTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Overview is required.")]
        public string Overview { get; set; } = string.Empty;
    }
}