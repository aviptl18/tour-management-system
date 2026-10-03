using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class LocationImageService : ILocationImageService
    {
        private readonly ILocationImageRepository _repository;

        public LocationImageService(ILocationImageRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LocationImage>> GetByLocationIdAsync(int locationId)
        {
            return await _repository.GetByLocationIdAsync(locationId);
        }

        public async Task AddAsync(LocationImage locationImage)
        {
            await _repository.AddAsync(locationImage);
        }
    }
}