using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.Models.Entities;

namespace Tour_Module.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllAsync();
        Task<List<Location>> GetAvailableForTourAsync();
        Task<Location?> GetByIdAsync(int locationId);
        Task CreateAsync(string name);
        Task UpdateAsync(int id, string name);
        Task DeactivateAsync(int id);
        Task ActivateAsync(int id);
        Task<List<Location>> SearchAsync(string? term);
    }
}