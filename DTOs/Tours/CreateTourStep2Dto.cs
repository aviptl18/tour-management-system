using System.ComponentModel.DataAnnotations;

namespace Tour_Module.DTOs.Tours
{
    public class CreateTourStep2Dto
    {
        public int DraftId { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public int LocationId { get; set; }
    }
}