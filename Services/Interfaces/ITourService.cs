using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.Models.Entities;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Services.Interfaces
{
    public interface ITourService
    {
        // ============================================================
        // READ OPERATIONS
        // ============================================================

        // Returns all tours for admin index page
        Task<IEnumerable<TourDto>> GetAllToursAsync();

        // Returns full tour details (used in admin details page)
        Task<AdminTourDetailVM?> GetAdminTourDetailAsync(int tourId);

        // ============================================================
        // WRITE OPERATIONS (CURRENT PHASE)
        // ============================================================

        // Creates a tour with ONLY core fields (no images, no addons)
        Task<int> CreateTourAsync(Tour tour);

        // Attaches images to an existing tour
        Task AddTourImagesAsync(
            int tourId,
            List<int> imageIds,
            int coverImageId
        );

        // ============================================================
        // LEGACY / TEMPORARY (COMMENTED — DO NOT USE)
        // ============================================================

        /*
        Task<int> AddTourAsync(
            int locationId,
            string tourTitle,
            string? duration,
            decimal basePrice,
            string? overview,
            List<int> imageIds,
            int coverImageId
        );
        */

        //Task UpdateTourAsync(UpdateTourDto dto);
        Task DeleteTourAsync(int tourId);
        Task<int> PublishTourAsync(int draftId, int adminId);
        Task<int> GetOrCreateEditDraftAsync(int tourId, int adminId);
        Task UpdateTourFromDraftAsync(int draftId, int adminId);
    }
}
