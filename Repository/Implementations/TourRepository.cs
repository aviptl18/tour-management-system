using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tour_Module.Data;
using Tour_Module.DTOs.Common;
using Tour_Module.DTOs.Tours;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;


namespace Tour_Module.Repository.Implementations
{
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDbContext _context;

        public TourRepository(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        // ============================================================
        // READ — GET ALL TOURS (ADMIN INDEX)
        // ============================================================
        public async Task<IEnumerable<TourDto>> GetAllToursAsync()
        {
            // Uses SP for optimized listing
            return await _context.TourDtos
                .FromSqlRaw("EXEC usp_GetAllTours")
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================================================
        // READ — GET TOUR DETAILS (ADMIN DETAILS PAGE)
        // ============================================================
        // ============================================================
        // READ — ADMIN TOUR DETAILS (READ MODEL)
        // ============================================================
        public async Task<AdminTourDetailVM?> GetAdminTourDetailsAsync(int tourId)
        {
            // 1️⃣ Tour + Location
            var tour = await (
                from t in _context.Tours
                join l in _context.Locations
                    on t.LocationId equals l.LocationId
                where t.TourId == tourId
                select new
                {
                    t.TourId,
                    t.TourTitle,
                    t.Overview,
                    t.Duration,
                    t.BasePrice,
                    t.IncludedTotal,
                    t.TotalPrice,
                    LocationName = l.LocationName
                }
            ).AsNoTracking().FirstOrDefaultAsync();

            if (tour == null)
                return null;

            // 2️⃣ Images (cover + gallery)
            var images = await (
                from ti in _context.TourImages
                join li in _context.LocationImages
                    on ti.ImageId equals li.ImageId
                where ti.TourId == tourId
                select new
                {
                    li.ImageUrl,
                    ti.IsCoverImage
                }
            ).AsNoTracking().ToListAsync();

            // 3️⃣ Allowed Add-ons
            var addOns = await (
                from tao in _context.TourAllowedOptions
                join od in _context.OptionDefinitions
                    on tao.OptionId equals od.OptionId
                where tao.TourId == tourId
                select od.OptionName
            ).AsNoTracking().ToListAsync();

            // 4️⃣ Map to ViewModel
            return new AdminTourDetailVM
            {
                TourId = tour.TourId,
                TourTitle = tour.TourTitle,
                LocationName = tour.LocationName,
                Overview = tour.Overview,
                Duration = tour.Duration,
                BasePrice = tour.BasePrice ?? 0,
                IncludedTotal = tour.IncludedTotal ?? 0,
                TotalPrice = tour.TotalPrice ?? 0,

                CoverImageUrl = images
                    .FirstOrDefault(x => x.IsCoverImage)?.ImageUrl,

                ImageUrls = images
                    .Select(x => x.ImageUrl)
                    .ToList(),

                AddOns = addOns
            };
        }

        // ============================================================
        // WRITE — CREATE TOUR (CORE DATA ONLY)
        // ============================================================
        public async Task<int> AddTourAsync(
            int locationId,
            string tourTitle,
            string? duration,
            decimal basePrice,
            string? overview)
        {
            // This SP ONLY inserts into Tours table
            var parameters = new[]
            {
                new SqlParameter("@LocationId", locationId),
                new SqlParameter("@TourTitle", tourTitle),
                new SqlParameter("@Duration", duration ?? (object)DBNull.Value),
                new SqlParameter("@BasePrice", basePrice),
                new SqlParameter("@Overview", overview ?? (object)DBNull.Value)
            };

            var result = await _context.Set<ScalarIntResult>()
                .FromSqlRaw(
                    "EXEC dbo.usp_AddTours @LocationId,@TourTitle,@Duration,@BasePrice,@Overview",
                    parameters)
                .AsNoTracking()
                .ToListAsync();

            if (!result.Any())
                throw new InvalidOperationException("Tour creation failed.");

            return result.First().NewTourId;
        }

        // ============================================================
        // WRITE — INSERT TOUR IMAGE (SNAPSHOT)
        // ============================================================
        public async Task InsertTourImageAsync(
            int tourId,
            int imageId,
            bool isCoverImage)
        {
            // 1️⃣ Insert TourImage using EF (TRACKED)
            var tourImage = new TourImages
            {
                TourId = tourId,
                ImageId = imageId,
                IsCoverImage = isCoverImage
            };

            _context.TourImages.Add(tourImage);
            await _context.SaveChangesAsync(); // 🔥 TourImageId is generated here

            // 2️⃣ If this is cover image, update Tours table
            if (isCoverImage)
            {
                var tour = await _context.Tours
                    .Where(t => t.TourId == tourId)
                    .FirstAsync();

                tour.CoverImageId = tourImage.TourImageId;

                await _context.SaveChangesAsync();
            }
        }



        // ============================================================
        // WRITE — UPDATE TOUR (LEGACY, WILL REFACTOR LATER)
        // ============================================================
        public async Task UpdateTourAsync(Tour tour)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_UpdateTour @p0,@p1,@p2,@p3,@p4,@p5,@p6",
                tour.TourId,
                tour.LocationId,
                tour.TourTitle,
                tour.Duration,
                tour.Overview
            );
        }

        // ============================================================
        // WRITE — DELETE TOUR
        // ============================================================
        public async Task DeleteTourAsync(int tourId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_DeleteTour @p0",
                tourId
            );
        }

        // PUBLISH TOUR

        public async Task<int> PublishTourAsync(int draftId, int adminId)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = "usp_PublishTour";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@DraftId", draftId));
            command.Parameters.Add(new SqlParameter("@AdminId", adminId));

            var outputId = new SqlParameter("@NewTourId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(outputId);

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            return (int)outputId.Value;
        }

        // Update Tour

        public async Task<int?> GetActiveDraftForTourAsync(int tourId)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = "usp_GetActiveDraftForTour";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@TourId", tourId));

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();

            if(result == null || result == DBNull.Value)
                return null;

            return Convert.ToInt32(result);
        }

        public async Task<int> CreateTourDraftFromLiveAsync(int tourId, int adminId)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = "usp_CreateTourDraftFromLive";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@TourId", tourId));
            command.Parameters.Add(new SqlParameter("@AdminId", adminId));

            var draftParam = new SqlParameter("@DraftId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(draftParam);

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            if (draftParam.Value == DBNull.Value)
                throw new Exception("Draft creation failed.");

            return Convert.ToInt32(draftParam.Value);
        }

        public async Task UpdateTourFromDraftAsync(int draftId, int adminId)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = "usp_UpdateTourFromDraft";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@DraftId", draftId));
            command.Parameters.Add(new SqlParameter("@AdminId", adminId));

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }
    }
}
