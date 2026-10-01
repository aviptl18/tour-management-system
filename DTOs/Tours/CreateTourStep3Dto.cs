using System.ComponentModel.DataAnnotations;

namespace Tour_Module.DTOs.Tours
{
    public class CreateTourStep3Dto
    {
        public int DraftId { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [MaxLength(100, ErrorMessage = "Duration is too long.")]
        public string? Duration { get; set; }

        //[Required(ErrorMessage = "Base price is required.")]
        //[Range(1, 10000000, ErrorMessage = "Base price must be greater than 0.")]
        //public decimal? BasePrice { get; set; }
    }
}
