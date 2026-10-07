using Tour_Module.DTOs.TourDiscovery;

namespace Tour_Module.Services.Interfaces
{
    public interface ITourDiscoveryService
    {
        Task<TourDiscoveryResultDto> SearchAsync(
            TourDiscoveryFilterDto filter);

        Task<TourDetailsDto?> GetDetailsAsync(
            int tourId);
    }
}