using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class TourDraftService : ITourDraftService
    {
        private readonly ITourDraftRepository _repository;
        private readonly IOptionRepository _optionRepository;

        public TourDraftService(
            ITourDraftRepository repository,
            IOptionRepository optionRepository)
        {
            _repository = repository;
            _optionRepository = optionRepository;
        }

        // =========================================================
        // DRAFT LIFECYCLE
        // =========================================================

        public async Task<int> StartDraftAsync(int adminId)
        {
            return await _repository.CreateDraftAsync(adminId);
        }

        public async Task<TourDraft?> GetDraftAsync(
            int draftId,
            int adminId)
        {
            return await _repository.GetDraftByIdAsync(
                draftId,
                adminId);
        }

        // =========================================================
        // STEP 1 - BASIC INFORMATION
        // =========================================================

        public async Task SaveStep1Async(
            int draftId,
            int adminId,
            string title,
            string overview)
        {
            await _repository.UpdateDraftBasicInfoAsync(
                draftId,
                adminId,
                title,
                overview);
        }

        // =========================================================
        // STEP 2 - LOCATION & IMAGES
        // =========================================================

        public async Task SaveStep2Async(
            int draftId,
            int adminId,
            int locationId,
            List<int> selectedImageIds,
            int coverImageId)
        {
            // Update draft location
            await _repository.UpdateDraftLocationAsync(
                draftId,
                adminId,
                locationId);

            // Replace existing draft images
            await _repository.DeleteAllDraftImagesAsync(
                draftId,
                adminId);

            // Add selected images
            foreach (var imageId in selectedImageIds)
            {
                bool isCover = imageId == coverImageId;

                await _repository.AddDraftImageAsync(
                    draftId,
                    adminId,
                    imageId,
                    isCover);
            }
        }

        public async Task<List<TourDraftImage>> GetDraftImagesAsync(
            int draftId,
            int adminId)
        {
            return await _repository.GetDraftImagesAsync(
                draftId,
                adminId);
        }

        // =========================================================
        // STEP 3 - PRICING
        // =========================================================

        public async Task<List<TourDraftPricingComponent>>
            GetStep3ComponentsAsync(
                int draftId,
                int adminId)
        {
            await _repository.EnsureBasePricingComponentAsync(
                draftId,
                adminId);

            return await _repository.GetDraftPricingComponentsAsync(
                draftId,
                adminId);
        }

        public async Task AddOptionAsync(
            int draftId,
            int adminId,
            int optionId)
        {
            // Get existing pricing components
            var components =
                await _repository.GetDraftPricingComponentsAsync(
                    draftId,
                    adminId);

            // Prevent duplicate option
            if (components.Any(c => c.OptionId == optionId))
            {
                return;
            }

            // Fetch option definition
            var option =
                await _optionRepository.GetByIdAsync(optionId);

            if (option == null || !option.IsActive)
            {
                throw new InvalidOperationException(
                    "Invalid option selected.");
            }

            await _repository.AddDraftPricingComponentAsync(
                draftId,
                adminId,
                option.OptionId,
                option.OptionName,
                option.BasePrice);
        }

        public async Task RemoveOptionAsync(
            int componentId,
            int adminId)
        {
            await _repository.RemoveDraftPricingComponentAsync(
                componentId,
                adminId);
        }

        public async Task UpdateBasePriceAsync(
            int draftId,
            int adminId,
            int componentId,
            decimal price)
        {
            if (draftId <= 0)
                throw new ArgumentException(
                    "Invalid draft.");

            if (adminId <= 0)
                throw new ArgumentException(
                    "Invalid admin.");

            if (componentId <= 0)
                throw new ArgumentException(
                    "Invalid pricing component.");

            if (price <= 0)
                throw new ArgumentException(
                    "Base price must be greater than 0.");

            await _repository.UpdateDraftBasePriceAsync(
                draftId,
                adminId,
                componentId,
                price);
        }

        // =========================================================
        // STEP 3 - DURATION
        // =========================================================

        public async Task SaveStep3Async(
            int draftId,
            int adminId,
            string duration)
        {
            await _repository.UpdateDraftDurationAsync(
                draftId,
                adminId,
                duration);
        }

        // =========================================================
        // PRICING VALIDATION
        // =========================================================

        public async Task<bool> ValidatePricingAsync(
            int draftId,
            int adminId)
        {
            var components =
                await _repository.GetDraftPricingComponentsAsync(
                    draftId,
                    adminId);

            var baseComponent =
                components.FirstOrDefault(
                    c => c.IsBaseComponent);

            // Base component must exist
            if (baseComponent == null)
            {
                return false;
            }

            // Base price must be greater than zero
            if (baseComponent.Price <= 0)
            {
                return false;
            }

            // No pricing component can have a negative price
            if (components.Any(c => c.Price < 0))
            {
                return false;
            }

            return true;
        }

        // =========================================================
        // COMPLETE STEP 3
        // =========================================================

        public async Task CompleteStep3Async(
            int draftId,
            int adminId)
        {
            await _repository.SetDraftStepAsync(
                draftId,
                adminId,
                3);
        }

        // =========================================================
        // COMPLETE DRAFT
        // =========================================================

        public async Task<bool> CompleteDraftAsync(
            int draftId,
            int adminId)
        {
            var draft =
                await _repository.GetDraftByIdAsync(
                    draftId,
                    adminId);

            if (draft == null)
            {
                return false;
            }

            // Step 1 validation
            if (string.IsNullOrWhiteSpace(
                draft.TourTitle))
            {
                return false;
            }

            // Step 2 validation
            if (draft.LocationId == null)
            {
                return false;
            }

            var images =
                await _repository.GetDraftImagesAsync(
                    draftId,
                    adminId);

            // A cover image is required
            if (!images.Any(i => i.IsCover))
            {
                return false;
            }

            // Step 3 pricing validation
            var pricingValid =
                await ValidatePricingAsync(
                    draftId,
                    adminId);

            if (!pricingValid)
            {
                return false;
            }

            // Ensure draft reaches step 3
            if (draft.DraftStep < 3)
            {
                await _repository.SetDraftStepAsync(
                    draftId,
                    adminId,
                    3);
            }

            // Mark draft as complete
            await _repository.UpdateDraftStatusAsync(
                draftId,
                "Complete");

            return true;
        }

        // =========================================================
        // CANCEL DRAFT
        // =========================================================

        public async Task CancelDraftAsync(
            int draftId,
            int adminId)
        {
            var draft =
                await _repository.GetDraftByIdAsync(
                    draftId,
                    adminId);

            if (draft == null)
            {
                return;
            }

            await _repository.UpdateDraftStatusAsync(
                draftId,
                "Cancelled");
        }
    }
}