using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;
using System.Globalization;

namespace Tour_Module.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;

        public LocationService(ILocationRepository repository)
        {
            _repository = repository;
        }

        private string Normalize(string input)
        {
            input = input.Trim().ToLowerInvariant();

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _repository.GetAllAsync(true);
        }

        public async Task<List<Location>> GetAvailableForTourAsync()
        {
            return await _repository.GetAllAsync(false);
        }

        public async Task<Location?> GetByIdAsync(int locationId)
        {
            var location = await _repository.GetByIdAsync(locationId);

            if (location == null)
                throw new Exception("Location not found");

            return location;
        }

        public async Task CreateAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Location name cannot be empty");

            name = Normalize(name);

            await _repository.AddAsync(name);
        }

        public async Task UpdateAsync(int id, string name)
        {
            if (id <= 0)
                throw new Exception("Invalid Location Id");

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Location name cannot be empty");

            name = Normalize(name);

            await _repository.UpdateAsync(id, name);
        }

        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Location Id");

            await _repository.DeactivateAsync(id);
        }

        public async Task ActivateAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Location Id");

            await _repository.ActivateAsync(id);
        }

        public async Task<List<Location>> SearchAsync(string? term)
        {
            return await _repository.SearchAsync(term);
        }
    }
}