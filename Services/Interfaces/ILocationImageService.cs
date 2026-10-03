using Tour_Module.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tour_Module.Services.Interfaces
{
    public interface ILocationImageService
    {
        Task<List<LocationImage>> GetByLocationIdAsync(int locationId);

        Task AddAsync(LocationImage locationImage);
    }
}