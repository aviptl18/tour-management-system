using Tour_Module.DTOs.Option;

namespace Tour_Module.ViewModels.Admin.Tours
{
    public class TourAddOnConfigVM
    {
        public int TourId { get; set; }

        // included in base Package 
        public List<TourAllowedOptionDto> IncludedOptions { get; set; }
            = new List<TourAllowedOptionDto>();

        // optional addons
        public List<TourAllowedOptionDto> OptionalOptions { get; set; }
            = new List<TourAllowedOptionDto>();

    }
}
