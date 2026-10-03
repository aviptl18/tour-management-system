using Tour_Module.DTOs.Option;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Services.Implementations
{
    public class TourAddOnService : ITourAddOnService
    {
        private readonly ITourAddOnRepository _repository;

        public TourAddOnService(ITourAddOnRepository repository)
        {
            _repository = repository;
        }

        public async Task<TourAddOnConfigurationViewModel> GetConfigurationAsync(int tourId)
        {
            var addOns = await _repository.GetConfigurationAsync(tourId);

            return new TourAddOnConfigurationViewModel
            {
                TourId = tourId,

                IncludedOptions = addOns
                    .Where(x => x.IsDefault)
                    .ToList(),

                OptionalAddOns = addOns
                    .Where(x => !x.IsDefault)
                    .ToList()
            };
        }

        public async Task<List<TourAddOnDto>> GetAvailableAddOnsAsync(int tourId)
        {
            return await _repository.GetAvailableAddOnsAsync(tourId);
        }

        public async Task AddOptionalAddOnAsync(int tourId, int optionId)
        {
            await _repository.AddOptionalAddOnAsync(
                tourId,
                optionId);
        }

        public async Task RemoveOptionalAddOnAsync(int tourId, int optionId)
        {
            await _repository.RemoveOptionalAddOnAsync(
                tourId,
                optionId);
        }
    }
}