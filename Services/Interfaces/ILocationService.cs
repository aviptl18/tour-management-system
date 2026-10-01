using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.Models.Entities;

namespace Tour_Module.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationsForAdminAsync();
        Task<List<Location>> GetActiveLocationsForToursAsync();
        Task<Location> GetLocationByIdAsync(int locationId);
        Task CreateLocationAsync(string name);
        Task UpdateLocationAsync(int id, string name, bool isActive);
        Task DeactivateLocationAsync(int id);
        Task<List<Location>> SearchLocationsAsync(string? term);
    }
}
