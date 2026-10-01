using System;

namespace Tour_Module.Models.Entities
{
    public class TourDraftPricingComponent
    {
        public int PricingComponentId { get; set; }

        public int DraftId { get; set; }

        public int? OptionId { get; set; }   // null for base component

        public string ComponentName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsBaseComponent { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
