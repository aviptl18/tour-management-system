using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Tour_Module.DTOs.Tours
{
    public class CreateTourStep2Dto
    {
        public int DraftId { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public int LocationId { get; set; }

        //[MinLength(1, ErrorMessage = "At least one image must be selected.")]
        //public List<int> SelectedImageIds { get; set; } = new();

        //[Required(ErrorMessage = "Cover image is required.")]
        //public int? CoverImageId { get; set; }
    }
}
