using Tour_Module.DTOs.TourDiscovery;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class TourDiscoveryService : ITourDiscoveryService
    {
        private readonly ITourDiscoveryRepository _repository;

        public TourDiscoveryService(
            ITourDiscoveryRepository repository)
        {
            _repository = repository;
        }

        public async Task<TourDiscoveryResultDto> SearchAsync(
            TourDiscoveryFilterDto filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            // PAGE NUMBER
            if (filter.PageNumber < 1)
            {
                filter.PageNumber = 1;
            }

            // PAGE SIZE
            if (filter.PageSize < 1)
            {
                filter.PageSize = 12;
            }

            if (filter.PageSize > 50)
            {
                filter.PageSize = 50;
            }

            // PRICE RANGE
            if (filter.MinPrice.HasValue &&
                filter.MinPrice.Value < 0)
            {
                filter.MinPrice = 0;
            }

            if (filter.MaxPrice.HasValue &&
                filter.MaxPrice.Value < 0)
            {
                filter.MaxPrice = 0;
            }

            if (filter.MinPrice.HasValue &&
                filter.MaxPrice.HasValue &&
                filter.MinPrice > filter.MaxPrice)
            {
                throw new ArgumentException(
                    "Minimum price cannot be greater than maximum price.");
            }

            // SORT
            var validSorts = new[]
            {
                "Newest",
                "PriceLowToHigh",
                "PriceHighToLow",
                "NameAscending",
                "NameDescending"
            };

            if (string.IsNullOrWhiteSpace(filter.SortBy))
            {
                filter.SortBy = "Newest";
            }
            else if (!validSorts.Contains(filter.SortBy))
            {
                filter.SortBy = "Newest";
            }

            return await _repository.SearchAsync(filter);
        }

        public async Task<TourDetailsDto?> GetDetailsAsync(
            int tourId)
        {
            if (tourId <= 0)
            {
                throw new ArgumentException(
                    "Invalid tour ID.");
            }

            return await _repository.GetDetailsAsync(tourId);
        }
    }
}