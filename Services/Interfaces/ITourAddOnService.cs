using Tour_Module.DTOs.Option;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Services.Interfaces
{
    public interface ITourAddOnService
    {
        Task<TourAddOnConfigurationViewModel> GetConfigurationAsync(int tourId);
        Task<List<TourAddOnDto>> GetAvailableAddOnsAsync(int tourId);
        Task AddOptionalAddOnAsync(int tourId, int optionId);
        Task RemoveOptionalAddOnAsync(int tourId, int optionId);
    }
}