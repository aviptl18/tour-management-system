using Tour_Module.DTOs.Tours;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourRepository
    {
        // =========================================================
        // READ
        // =========================================================

        Task<List<TourDto>> GetAllAsync();

        Task<TourDetailsViewModel?> GetDetailsAsync(int tourId);


        // =========================================================
        // PUBLISH
        // =========================================================

        Task<int> PublishFromDraftAsync(
            int draftId,
            int adminId);


        // =========================================================
        // EDIT EXISTING TOUR
        // =========================================================

        Task<int?> GetActiveDraftIdAsync(
            int tourId);

        Task<int> CreateDraftFromTourAsync(
            int tourId,
            int adminId);

        Task UpdateFromDraftAsync(
            int draftId,
            int adminId);

        // Delete

        Task DeleteAsync(int tourId);
    }
}