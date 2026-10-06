using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Services.Implementations
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository
                ?? throw new ArgumentNullException(nameof(tourRepository));
        }

        // ============================================================
        // READ — GET ALL TOURS
        // Purpose:
        //   - Returns all tours for the admin tour listing page.
        // ============================================================

        public async Task<IEnumerable<TourDto>> GetAllToursAsync()
        {
            return await _tourRepository.GetAllAsync();
        }


        // ============================================================
        // READ — GET TOUR DETAILS
        // Purpose:
        //   - Returns details of one existing tour.
        // ============================================================

        public async Task<TourDetailsViewModel?> GetTourDetailsAsync(
            int tourId)
        {
            if (tourId <= 0)
                return null;

            return await _tourRepository.GetDetailsAsync(tourId);
        }


        // ============================================================
        // PUBLISH — CREATE LIVE TOUR FROM COMPLETED DRAFT
        // Purpose:
        //   - Publishes a completed draft.
        //   - Creates a new live tour.
        //   - Returns the newly created TourId.
        // ============================================================

        public async Task<int> PublishTourAsync(
            int draftId,
            int adminId)
        {
            if (draftId <= 0)
                throw new InvalidOperationException(
                    "Invalid draft.");

            if (adminId <= 0)
                throw new InvalidOperationException(
                    "Invalid admin.");

            return await _tourRepository.PublishFromDraftAsync(
                draftId,
                adminId);
        }


        // ============================================================
        // EDIT — GET OR CREATE EDIT DRAFT
        // Purpose:
        //   - Checks whether an active edit draft already exists.
        //   - Returns the existing draft when found.
        //   - Creates a new draft from the live tour otherwise.
        // ============================================================

        public async Task<int> GetOrCreateEditDraftAsync(
            int tourId,
            int adminId)
        {
            if (tourId <= 0)
                throw new InvalidOperationException(
                    "Invalid tour.");

            if (adminId <= 0)
                throw new InvalidOperationException(
                    "Invalid admin.");

            var draftId =
                await _tourRepository.GetActiveDraftIdAsync(
                    tourId);

            if (draftId.HasValue)
                return draftId.Value;

            return await _tourRepository.CreateDraftFromTourAsync(
                tourId,
                adminId);
        }


        // ============================================================
        // EDIT — UPDATE LIVE TOUR FROM COMPLETED DRAFT
        // Purpose:
        //   - Applies the completed edit draft to the existing tour.
        // ============================================================

        public async Task UpdateTourFromDraftAsync(
            int draftId,
            int adminId)
        {
            if (draftId <= 0)
                throw new InvalidOperationException(
                    "Invalid draft.");

            if (adminId <= 0)
                throw new InvalidOperationException(
                    "Invalid admin.");

            await _tourRepository.UpdateFromDraftAsync(
                draftId,
                adminId);
        }

        // Delete Tour

        public async Task DeleteTourAsync(int tourId)
        {
            if (tourId <= 0)
                throw new ArgumentException(
                    "Invalid tour.");

            await _tourRepository.DeleteAsync(tourId);
        }
    }
}