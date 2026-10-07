using System.Threading.Tasks;
using Tour_Module.DTOs.TourDiscovery;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourDiscoveryRepository
    {
        Task<TourDiscoveryResultDto> SearchAsync(
            TourDiscoveryFilterDto filter);

        Task<TourDetailsDto?> GetDetailsAsync(
            int tourId);
    }
}