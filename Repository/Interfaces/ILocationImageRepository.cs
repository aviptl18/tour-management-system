using Tour_Module.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tour_Module.Repository.Interfaces
{
    public interface ILocationImageRepository
    {
        Task<List<LocationImage>> GetByLocationIdAsync(int locationId);
        Task AddAsync(LocationImage locationImage);
    }
}