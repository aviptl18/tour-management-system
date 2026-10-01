using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tour_Module.DTOs.Tours;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;
using Tour_Module.Data;
using Tour_Module.ViewModels.Admin.Tours;


namespace Tour_Module.Services.Implementations
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ApplicationDbContext _context;

        public TourService(
            ITourRepository tourRepository,
            ApplicationDbContext context)
        {
            _tourRepository = tourRepository
                ?? throw new ArgumentNullException(nameof(tourRepository));

            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        // ============================================================
        // READ — GET ALL TOURS
        // Purpose:
        //   - Used by admin tour listing
        // ============================================================
        public async Task<IEnumerable<TourDto>> GetAllToursAsync()
        {
            return await _tourRepository.GetAllToursAsync();
        }

        // ============================================================
        // READ — GET TOUR DETAILS
        // Purpose:
        //   - Used by admin tour details page
        // ============================================================
        public async Task<AdminTourDetailVM?> GetAdminTourDetailAsync(int tourId)
        {
            if (tourId <= 0)
                return null;

            return await _tourRepository.GetAdminTourDetailsAsync(tourId);
        }

        // ============================================================
        // WRITE — CREATE TOUR (CORE DATA ONLY)
        // Purpose:
        //   - Creates Tour row only
        //   - No images
        //   - No addons
        // ============================================================
        public async Task<int> CreateTourAsync(Tour tour)
        {
            if (tour == null)
                throw new ArgumentNullException(nameof(tour));

            if (string.IsNullOrWhiteSpace(tour.TourTitle))
                throw new InvalidOperationException("Tour title is required.");

            if (tour.LocationId <= 0)
                throw new InvalidOperationException("Location is required.");

            if (tour.BasePrice == null || tour.BasePrice <= 0)
                throw new InvalidOperationException("Base price must be greater than zero.");

            return await _tourRepository.AddTourAsync(
                tour.LocationId,
                tour.TourTitle,
                tour.Duration,
                tour.BasePrice.Value,
                tour.Overview
            );
        }

        // ============================================================
        // WRITE — ADD TOUR IMAGES
        // Purpose:
        //   - Attaches selected images to an existing tour
        //   - Sets cover image explicitly
        // ============================================================
        public async Task AddTourImagesAsync(
            int tourId,
            List<int> imageIds,
            int coverImageId)
        {
            if (tourId <= 0)
                throw new InvalidOperationException("Invalid tour.");

            if (imageIds == null || !imageIds.Any())
                throw new InvalidOperationException("At least one image must be selected.");

            if (!imageIds.Contains(coverImageId))
                throw new InvalidOperationException("Cover image must be part of selected images.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var imageId in imageIds.Distinct())
                {
                    await _tourRepository.InsertTourImageAsync(
                        tourId,
                        imageId,
                        imageId == coverImageId
                    );
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // ============================================================
        // LEGACY — UPDATE (WILL BE REFACTORED LATER)
        // ============================================================
        //public async Task UpdateTourAsync(UpdateTourDto dto)
        //{
        //    if (dto == null)
        //        throw new ArgumentNullException(nameof(dto));

        //    var tour = new Tour
        //    {
        //        TourId = dto.TourId,
        //        LocationId = dto.LocationId,
        //        TourTitle = dto.TourTitle,
        //        Duration = dto.Duration,
        //        Price = dto.Price,
        //        Overview = dto.Overview,
        //        ImageUrl = dto.ImageUrl
        //    };

        //    await _tourRepository.UpdateTourAsync(tour);
        //}

        // ============================================================
        // LEGACY — DELETE
        // ============================================================
        public async Task DeleteTourAsync(int tourId)
        {
            await _tourRepository.DeleteTourAsync(tourId);
        }

        // PUBLISH

        public async Task<int> PublishTourAsync(int draftId, int adminId)
        {
            return await _tourRepository.PublishTourAsync(draftId, adminId);
        }

        // Update tour

        public async Task<int> GetOrCreateEditDraftAsync(int tourId, int adminId)
        {
            var draftId = await _tourRepository.GetActiveDraftForTourAsync(tourId);

            if (draftId.HasValue)
                return draftId.Value;

            return await _tourRepository.CreateTourDraftFromLiveAsync(tourId, adminId);
        }

        public async Task UpdateTourFromDraftAsync(int draftId, int adminId)
        {
            await _tourRepository.UpdateTourFromDraftAsync(draftId, adminId);
        }
    }
}
