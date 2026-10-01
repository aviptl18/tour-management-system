using Tour_Module.Models.Entities;
using Tour_Module.Repository.Implementations;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class TourDraftService : ITourDraftService
    {
        private readonly ITourDraftRepository _repository;
        private readonly IOptionRepository _optionRepository;

        public TourDraftService(ITourDraftRepository repository, IOptionRepository optionRepository)
        {
            _repository = repository;
            _optionRepository = optionRepository;
        }

        public async Task<int> StartDraftAsync(int adminId)
        {
            return await _repository.CreateDraftAsync(adminId);
        }

        public async Task<TourDraft?> GetDraftAsync(int draftId, int adminId)
        {
            return await _repository.GetDraftByIdAsync(draftId, adminId);
        }

        public async Task SaveStep1Async(int draftId, int adminId, string title, string overview)
        {
            await _repository.UpdateStep1Async(draftId, adminId, title, overview);
        }

        public async Task SaveStep2Async(int draftId, int adminId, int locationId,
            List<int> selectedImageIds, int coverImageId)
        {
            // update location
            await _repository.UpdateDraftLocationAsync(draftId, adminId, locationId);

            // delete existing draft images (overwrite)
            await _repository.DeleteDraftImagesAsync(draftId);

            // insert selected images
            foreach (var imageId in selectedImageIds)
            {
                bool isCover = imageId == coverImageId;
                await _repository.AddDraftImageAsync(draftId, imageId, isCover);
            }

        }

        public async Task<List<TourDraftImage>> GetDraftImagesAsync(int draftId)
        {
            return await _repository.GetDraftImagesAsync(draftId);
        }

        public async Task<List<TourDraftPricingComponent>> GetStep3ComponentsAsync(int draftId)
        {
            await _repository.EnsureBaseComponentAsync(draftId);

            return await _repository.GetPricingComponentsAsync(draftId);
        }

        public async Task AddOptionAsync(int draftId, int optionId)
        {
            // Check existing components
            var components = await _repository.GetPricingComponentsAsync(draftId);

            if(components.Any(c => c.OptionId == optionId))
                    return; // prevent duplicate

            // prevent adding option as base
            if (components.Any(c => c.IsBaseComponent && c.OptionId == optionId))
                return;

            // fetch option from definition
            var option = await _optionRepository.GetByIdAsync(optionId);

            if (option == null || !option.IsActive)
                throw new InvalidOperationException("Invalid option selected.");

            await _repository.AddPricingComponentAsync(
                draftId,
                option.OptionId,
                option.OptionName,
                option.BasePrice,
                false
            );
        }

        public async Task RemoveOptionAsync(int componentId)
        {
            await _repository.RemovePricingComponentAsync(componentId);
        }

        public async Task UpdateComponentPriceAsync(int componentId, decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Base price must be greater than 0.");

            await _repository.UpdateComponentPriceAsync(componentId, price);
        }

        public async Task SaveStep3Async(int draftId, string duration)
        {
            await _repository.UpdateStep3Async(draftId, duration);
        }


        public async Task<bool> ValidatePricingAsync(int draftId)
        {
            var components = await _repository.GetPricingComponentsAsync(draftId);

            var baseComponent = components.FirstOrDefault(c => c.IsBaseComponent);

            // must exist
            if (baseComponent == null)
                return false;

            // must have valid base price
            if (baseComponent.Price <= 0)
                return false;

            // no negative pricing
            if (components.Any(c => c.Price < 0))
                return false;

            return true;
        }

        public async Task CompleteStep3Async(int draftId)
        {
            await _repository.SetDraftStepAsync(draftId, 3);
        }

        public async Task<bool> CompleteDraftAsync(int draftId, int adminId)
        {
            var draft = await _repository.GetDraftByIdAsync(draftId, adminId);

            if (draft == null)
                return false;

            if (string.IsNullOrWhiteSpace(draft.TourTitle))
                return false;

            if (draft.LocationId == null)
                return false;

            var images = await _repository.GetDraftImagesAsync(draftId);
            if (!images.Any(i => i.IsCover))
                return false;

            var pricingValid = await ValidatePricingAsync(draftId);
            if (!pricingValid)
                return false;

            // ⭐ SAVE STEP 3 DATA IF NOT COMPLETED
            if (draft.DraftStep < 3)
            {
                await _repository.SetDraftStepAsync(draftId, 3);
            }

            await _repository.UpdateDraftStatusAsync(draftId, "Complete");

            return true;
        }

        public async Task CancelDraftAsync(int draftId, int adminId)
        {
            var draft = await _repository.GetDraftByIdAsync(draftId, adminId);

            if(draft == null) 
                return;

            await _repository.UpdateDraftStatusAsync(draftId, "Cancelled");
        }



    }

}
