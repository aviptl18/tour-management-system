using Tour_Module.Models.Entities;

namespace Tour_Module.DTOs.Tours.View
{
    public class CreateTourStep3ViewDto
    {
        public int DraftId { get; set; }

        public string? Duration { get; set; }
        //public decimal? BasePrice { get; set; }

        public List<TourDraftPricingComponent> Components { get; set; } = new();

        public List<Tour_Module.Models.Entities.Option> AvailableOptions { get; set; } = new();
        public string Status { get; set; }

        public int? OriginalTourId { get; set; }
    }
}
