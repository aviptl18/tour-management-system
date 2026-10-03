using Tour_Module.DTOs.Option;

namespace Tour_Module.ViewModels.Tour
{
    public class TourAddOnConfigViewModel
    {
        // Identifies which tour we are configuring
        public int TourId { get; set; }

        // All ACTIVE Add-Ons from OptionDefinition
        public List<OptionDto> ActiveAddOns { get; set; } = new();

        // OptionIds that are ALLOWED for this tour
        public HashSet<int> AllowedOptionIds { get; set; } = new();

        // Distinct categories for filter dropdown
        public List<string> Categories { get; set; } = new();

        // Selected category (for filtering)
        public string? SelectedCategory { get; set; }

        // Search text (for filtering)
        public string? SearchText { get; set; }
    }
}


// NEEDS TO DELETE THIS FILE