using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tour_Module.ViewModels.Admin.Tours
{
    public class CreateStep2ViewModel
    {
        public int DraftId { get; set; }

        public int LocationId { get; set; }

        public List<SelectListItem> Locations { get; set; }
            = new List<SelectListItem>();

        public List<int> SelectedImageIds { get; set; }
            = new List<int>();

        public int? CoverImageId { get; set; }

        public int? OriginalTourId { get; set; }
    }
}