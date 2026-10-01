using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tour_Module.DTOs.Tours.View
{
    public class CreateTourStep2ViewDto
    {
        public int DraftId { get; set; }

        // Dropdown
        public int LocationId { get; set; }
        public List<SelectListItem> Locations { get; set; } = new();

        // Gallery state (render-only)
        public List<int> SelectedImageIds { get; set; } = new();
        public int? CoverImageId { get; set; }
        public int? OriginalTourId { get; set; }

    }
}
