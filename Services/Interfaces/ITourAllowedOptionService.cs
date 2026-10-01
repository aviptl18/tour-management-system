using Tour_Module.DTOs.Option;
using Tour_Module.ViewModels.Admin.Tours;
using Tour_Module.ViewModels.Tour;

public interface ITourAllowedOptionService
{
    // For configuration page
    Task<TourAddOnConfigVM> GetConfigurationDataAsync(int tourId);

    // For dynamic dropdown loading
    Task<List<TourAllowedOptionDto>>
        GetAvailableOptionalOptionsForTourAsync(int tourId);

    // Add optional add-on
    Task AllowOptionForTourAsync(int tourId, int optionId, bool isDefault);

    // Remove optional add-on
    Task RemoveOptionFromTourAsync(int tourId, int optionId);
}