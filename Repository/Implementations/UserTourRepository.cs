using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tour_Module.Data;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class UserTourRepository : IUserTourRepository
    {
        private readonly ApplicationDbContext _context;

        public UserTourRepository(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserTourDetailsDto?> GetUserTourDetailsAsync(int tourId)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = "usp_GetUserTourDetails";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@TourId", tourId));

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            // =====================================================
            // 1️⃣ CORE TOUR DETAILS
            // =====================================================
            if (!await reader.ReadAsync())
                return null;

            var dto = new UserTourDetailsDto
            {
                TourId = reader.GetInt32(0),
                TourTitle = reader.GetString(1),
                LocationName = reader.GetString(2),
                Overview = reader.IsDBNull(3) ? null : reader.GetString(3),
                Duration = reader.IsDBNull(4) ? null : reader.GetString(4),
                BasePrice = reader.GetDecimal(5),
                IncludedTotal = reader.GetDecimal(6),
                TotalPrice = reader.GetDecimal(7)
            };

            // =====================================================
            // 2️⃣ TOUR IMAGES
            // =====================================================
            await reader.NextResultAsync();

            while (await reader.ReadAsync())
            {
                var imageUrl = reader.GetString(0);
                var isCover = reader.GetBoolean(1);

                dto.ImageUrls.Add(imageUrl);

                if (isCover)
                    dto.CoverImageUrl = imageUrl;
            }

            // =====================================================
            // 3️⃣ ADD-ONS
            // =====================================================
            await reader.NextResultAsync();

            while (await reader.ReadAsync())
            {
                dto.AddOns.Add(reader.GetString(0));
            }

            return dto;
        }
    }
}
