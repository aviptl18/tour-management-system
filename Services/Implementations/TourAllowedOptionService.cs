using Tour_Module.DTOs.Option;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;
using Tour_Module.ViewModels.Tour;

namespace Tour_Module.Services.Implementations
{
    public class TourAllowedOptionService
        : ITourAllowedOptionService
    {
        private readonly ITourAllowedOptionRepository _repository;

        public TourAllowedOptionService(
            ITourAllowedOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<TourAddOnConfigVM>
            GetConfigurationDataAsync(int tourId)
        {
            var allowed = await _repository
                .GetAllowedOptionsForTourAsync(tourId);

            return new TourAddOnConfigVM
            {
                TourId = tourId,
                IncludedOptions = allowed
                    .Where(o => o.IsDefault)
                    .ToList(),

                OptionalOptions = allowed
                    .Where(o => !o.IsDefault)
                    .ToList()
            };
        }

        public async Task<List<TourAllowedOptionDto>>
            GetAvailableOptionalOptionsForTourAsync(int tourId)
        {
            return await _repository
                .GetAvailableOptionalOptionsForTourAsync(tourId);
        }

        public async Task AllowOptionForTourAsync(
            int tourId, int optionId, bool isDefault)
        {
            await _repository
                .AddAllowedOptionAsync(tourId, optionId, isDefault);
        }

        public async Task RemoveOptionFromTourAsync(
            int tourId, int optionId)
        {
            await _repository
                .RemoveAllowedOptionAsync(tourId, optionId);
        }
    }
}