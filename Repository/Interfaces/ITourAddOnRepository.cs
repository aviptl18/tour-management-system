using Tour_Module.DTOs.Option;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourAddOnRepository
    {
        Task<List<TourAddOnDto>> GetConfigurationAsync(int tourId);
        Task<List<TourAddOnDto>> GetAvailableAddOnsAsync(int tourId);
        Task AddOptionalAddOnAsync(int tourId, int optionId);
        Task RemoveOptionalAddOnAsync(int tourId, int optionId);
    }
}