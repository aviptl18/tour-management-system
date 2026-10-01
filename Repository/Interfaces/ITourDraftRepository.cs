using Tour_Module.Models.Entities;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourDraftRepository
    {
        Task<int> CreateDraftAsync(int adminId);
        Task<TourDraft?> GetDraftByIdAsync(int draftId, int adminId);
        Task UpdateStep1Async(int draftId, int adminId, string title, string overview);
        Task UpdateDraftLocationAsync(int draftId, int adminId, int locationId);
        Task DeleteDraftImagesAsync(int draftId);
        Task AddDraftImageAsync(int draftId, int locationImageId, bool isCover);
        Task<List<TourDraftImage>> GetDraftImagesAsync(int draftId);
        Task EnsureBaseComponentAsync(int draftId);
        Task<List<TourDraftPricingComponent>> GetPricingComponentsAsync(int draftId);
        //Task<TourDraftPricingComponent?> GetComponentByIdAsync(int componentId);
        Task AddPricingComponentAsync(int draftId, int optionId, string componentName, decimal price, bool isBaseComponent);
        Task RemovePricingComponentAsync(int componentId);
        Task UpdateComponentPriceAsync(int componentId, decimal price);
        Task SetDraftStepAsync(int draftId, int step);
        Task UpdateStep3Async(int draftId, string duration);
        Task UpdateDraftStatusAsync(int draftId, string status);   
    }

}
