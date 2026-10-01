using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tour_Module.Services.Implementations
{
    public class LocationImageService : ILocationImageService
    {
        private readonly ILocationImageRepository _locationImageRepository;
        public LocationImageService(ILocationImageRepository locationImageRepository)
        {
            _locationImageRepository = locationImageRepository;
        }

        public async Task<List<LocationImage>> GetImagesByLocationIdAsync(int locationId)
        {
            return await _locationImageRepository.GetImagesByLocationIdAsync(locationId);
        }

        public async Task AddLocationImageAsync(LocationImage locationImage)
        {
            await _locationImageRepository.AddLocationImageAsync(locationImage);
        }

    }
}
