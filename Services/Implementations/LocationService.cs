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

        public async Task<List<Location>> GetAllLocationsForAdminAsync()
        {
            return await _repository.GetAllAsync(true);
        }
        public async Task<List<Location>> GetActiveLocationsForToursAsync()
        {
            return await _repository.GetAllAsync(false);
        }
        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            var location = await _repository.GetByIdAsync(locationId);

            if (location == null)
                throw new Exception("Location not found");

            return location;
        }
        public async Task CreateLocationAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Location name cannot be empty");

            name = Normalize(name);
            await _repository.InsertAsync(name);
        }
        public async Task UpdateLocationAsync(int id, string name, bool isActive)
        {
            if (id <= 0)
                throw new Exception("Invalid Location Id");

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Location name cannot be empty");

            if (!isActive)
                throw new Exception("Use Deactivate action to deactivate a location.");

            name = Normalize(name);
            await _repository.UpdateAsync(id, name, true);
        }
        public async Task DeactivateLocationAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Location Id");

            await _repository.SetInactiveAsync(id);
        }

        public async Task<List<Location>> SearchLocationsAsync(string? term)
        {
            return await _repository.SearchLocationsAsync(term);
        }
    }
}
