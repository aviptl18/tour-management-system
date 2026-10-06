using Tour_Module.Models.Entities;

namespace Tour_Module.Services.Interfaces
{
    public interface ITourDraftService
    {
        // =========================================================
        // DRAFT LIFECYCLE
        // =========================================================

        Task<int> StartDraftAsync(int adminId);

        Task<TourDraft?> GetDraftAsync(
            int draftId,
            int adminId);

        // =========================================================
        // STEP 1 - BASIC INFORMATION
        // =========================================================

        Task SaveStep1Async(
            int draftId,
            int adminId,
            string title,
            string overview);

        // =========================================================
        // STEP 2 - LOCATION & IMAGES
        // =========================================================

        Task SaveStep2Async(
            int draftId,
            int adminId,
            int locationId,
            List<int> selectedImageIds,
            int coverImageId);

        Task<List<TourDraftImage>> GetDraftImagesAsync(
            int draftId,
            int adminId);

        // =========================================================
        // STEP 3 - PRICING & DURATION
        // =========================================================

        Task<List<TourDraftPricingComponent>>
            GetStep3ComponentsAsync(
                int draftId,
                int adminId);

        Task AddOptionAsync(
            int draftId,
            int adminId,
            int optionId);

        Task RemoveOptionAsync(
            int componentId,
            int adminId);

        Task UpdateBasePriceAsync(
            int draftId,
            int adminId,
            int componentId,
            decimal price);

        Task SaveStep3Async(
            int draftId,
            int adminId,
            string duration);

        Task<bool> ValidatePricingAsync(
            int draftId,
            int adminId);

        Task CompleteStep3Async(
            int draftId,
            int adminId);

        // =========================================================
        // DRAFT COMPLETION / CANCELLATION
        // =========================================================

        Task<bool> CompleteDraftAsync(
            int draftId,
            int adminId);

        Task CancelDraftAsync(
            int draftId,
            int adminId);
    }
}