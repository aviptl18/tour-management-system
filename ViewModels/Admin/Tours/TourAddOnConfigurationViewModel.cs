using Tour_Module.DTOs.Option;

namespace Tour_Module.ViewModels.Admin.Tours
{
    public class TourAddOnConfigurationViewModel
    {
        public int TourId { get; set; }

        // Options included in the base package
        public List<TourAddOnDto> IncludedOptions { get; set; }
            = new List<TourAddOnDto>();

        // Optional services available as Add-Ons
        public List<TourAddOnDto> OptionalAddOns { get; set; }
            = new List<TourAddOnDto>();
    }
}