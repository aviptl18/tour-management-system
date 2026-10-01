using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.Models.Entities;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Repository.Interfaces
{
    public interface ITourRepository
    {
        // ============================================================
        // READ OPERATIONS
        // ============================================================

        // Admin: fetch all tours for index page
        Task<IEnumerable<TourDto>> GetAllToursAsync();

        // Admin: fetch full tour details (READ MODEL)
        Task<AdminTourDetailVM?> GetAdminTourDetailsAsync(int tourId);

        // ============================================================
        // WRITE OPERATIONS
        // ============================================================

        // Create tour (core data only)
        Task<int> AddTourAsync(
            int locationId,
            string tourTitle,
            string? duration,
            decimal basePrice,
            string? overview
        );

        // Attach images to tour
        Task InsertTourImageAsync(
            int tourId,
            int imageId,
            bool isCoverImage
        );

        // Update core tour fields (legacy – refactor later)
        Task UpdateTourAsync(Tour tour);

        // Delete / deactivate tour
        Task DeleteTourAsync(int tourId);

        Task<int> PublishTourAsync(int draftId, int tourId);
        Task<int?> GetActiveDraftForTourAsync(int tourId);
        Task<int> CreateTourDraftFromLiveAsync(int tourId, int adminId);
        Task UpdateTourFromDraftAsync(int draftId, int adminId);

    }
}
