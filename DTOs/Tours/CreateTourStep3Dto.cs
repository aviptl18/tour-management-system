using System.ComponentModel.DataAnnotations;

namespace Tour_Module.DTOs.Tours
{
    public class CreateTourStep3Dto
    {
        public int DraftId { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [MaxLength(100, ErrorMessage = "Duration is too long.")]
        public string? Duration { get; set; }
    }
}