using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Services.Interfaces
{
    public interface ITourService
    {
        // ============================================================
        // READ OPERATIONS
        // ============================================================

        // Returns all tours for the admin tour listing page.
        Task<IEnumerable<TourDto>> GetAllToursAsync();

        // Returns details of one tour for the admin details page.
        Task<TourDetailsViewModel?> GetTourDetailsAsync(int tourId);


        // ============================================================
        // PUBLISH
        // ============================================================

        // Publishes a completed draft and creates a new live tour.
        Task<int> PublishTourAsync(
            int draftId,
            int adminId);


        // ============================================================
        // EDIT EXISTING TOUR
        // ============================================================

        // Gets an existing active edit draft or creates one from
        // the live tour when no active draft exists.
        Task<int> GetOrCreateEditDraftAsync(
            int tourId,
            int adminId);

        // Applies a completed edit draft to the existing live tour.
        Task UpdateTourFromDraftAsync(
            int draftId,
            int adminId);

        Task DeleteTourAsync(int tourId);

    }
}