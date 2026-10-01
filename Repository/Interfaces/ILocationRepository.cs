using Tour_Module.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tour_Module.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllAsync(bool includeInactive);
        Task<Location?> GetByIdAsync(int locationId);

        Task InsertAsync(string locationName);
        Task UpdateAsync(int locationId, string locationName, bool isActive);
        Task SetInactiveAsync(int locationId);
        Task<List<Location>> SearchLocationsAsync(string? searchTerm);

    }
}
