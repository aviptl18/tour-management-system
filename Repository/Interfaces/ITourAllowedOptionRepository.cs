using Tour_Module.DTOs.Option;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourAllowedOptionRepository
    {
        Task<List<TourAllowedOptionDto>> GetAllowedOptionsForTourAsync(int tourId);
        Task<List<TourAllowedOptionDto>> GetAvailableOptionalOptionsForTourAsync(int tourId);
        Task AddAllowedOptionAsync(int tourId, int optionId, bool isDefault);
        Task RemoveAllowedOptionAsync(int tourId, int optionId);
    }
}
