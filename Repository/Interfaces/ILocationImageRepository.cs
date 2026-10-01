using Tour_Module.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tour_Module.Repository.Interfaces
{
    public interface ILocationImageRepository
    {
        Task<List<LocationImage>> GetImagesByLocationIdAsync(int locationId);
        Task AddLocationImageAsync(LocationImage locationImage);
    }   
}
