using Tour_Module.Models.Entities;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourDraftRepository
    {
        // DRAFT LIFECYCLE

        Task<int> CreateDraftAsync(int adminId);

        Task<TourDraft?> GetDraftByIdAsync(
            int draftId,
            int adminId);

        // STEP 1 - BASIC INFORMATION

        Task UpdateDraftBasicInfoAsync(
            int draftId,
            int adminId,
            string title,
            string overview);

        // STEP 2 - LOCATION

        Task UpdateDraftLocationAsync(
            int draftId,
            int adminId,
            int locationId);

        // STEP 2 - IMAGES

        Task DeleteAllDraftImagesAsync(
            int draftId,
            int adminId);

        Task AddDraftImageAsync(
            int draftId,
            int adminId,
            int locationImageId,
            bool isCover);

        Task<List<TourDraftImage>> GetDraftImagesAsync(
            int draftId,
            int adminId);

        // STEP 3 - PRICING

        Task EnsureBasePricingComponentAsync(
            int draftId,
            int adminId);

        Task<List<TourDraftPricingComponent>>
            GetDraftPricingComponentsAsync(
                int draftId,
                int adminId);

        Task AddDraftPricingComponentAsync(
            int draftId,
            int adminId,
            int optionId,
            string componentName,
            decimal price);

        Task RemoveDraftPricingComponentAsync(
            int pricingComponentId,
            int adminId);

        Task UpdateDraftBasePriceAsync(
            int draftId,
            int adminId,
            int pricingComponentId,
            decimal price);

        // DRAFT PROGRESS

        Task SetDraftStepAsync(
            int draftId,
            int adminId,
            int step);

        Task UpdateDraftDurationAsync(
            int draftId,
            int adminId,
            string duration);

        // DRAFT STATUS

        Task UpdateDraftStatusAsync(
            int draftId,
            string status);

        // EDIT EXISTING TOUR

        Task<int?> GetActiveDraftForTourAsync(
            int tourId);

        Task<int> CreateDraftFromTourAsync(
            int tourId,
            int adminId);
    }
}