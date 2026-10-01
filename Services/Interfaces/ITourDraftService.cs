using Tour_Module.Models.Entities;

namespace Tour_Module.Services.Interfaces
{
    public interface ITourDraftService
    {
        Task<int> StartDraftAsync(int adminId);
        Task<TourDraft?> GetDraftAsync(int draftId, int adminId);
        Task SaveStep1Async(int draftId, int adminId, string title, string overview);
        Task SaveStep2Async(int draftId, int adminId, int locationId,
            List<int> selectedImageIds, int coverImageId);
        Task<List<TourDraftImage>> GetDraftImagesAsync(int draftId);
        Task<List<TourDraftPricingComponent>> GetStep3ComponentsAsync(int draftId);
        Task AddOptionAsync(int draftId, int optionId);
        Task RemoveOptionAsync(int componentId);
        Task UpdateComponentPriceAsync(int componentId, decimal price);
        Task SaveStep3Async(int draftId, string duration);
        Task<bool> ValidatePricingAsync(int draftId);
        Task CompleteStep3Async(int draftId);
        Task<bool> CompleteDraftAsync(int draftId, int adminId);
        Task CancelDraftAsync(int draftId, int adminId);
    }

}
