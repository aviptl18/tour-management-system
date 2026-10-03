using Tour_Module.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tour_Module.Repository.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllAsync(bool includeInactive);
        Task<Location?> GetByIdAsync(int locationId);
        Task AddAsync(string locationName);
        Task UpdateAsync(int locationId, string locationName);
        Task DeactivateAsync(int locationId);
        Task ActivateAsync(int locationId);
        Task<List<Location>> SearchAsync(string? searchTerm);

    }
}
