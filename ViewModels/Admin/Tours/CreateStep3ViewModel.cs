using Tour_Module.Models.Entities;

namespace Tour_Module.ViewModels.Admin.Tours
{
    public class CreateStep3ViewModel
    {
        public int DraftId { get; set; }

        public string? Duration { get; set; }

        public List<TourDraftPricingComponent> Components { get; set; }
            = new List<TourDraftPricingComponent>();

        public List<Option> AvailableOptions { get; set; }
            = new List<Option>();

        public string Status { get; set; } = string.Empty;

        public int? OriginalTourId { get; set; }
    }
}