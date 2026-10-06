using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class UserTourRepository : IUserTourRepository
    {
        private readonly string _connectionString;

        public UserTourRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserTourDetailsDto?> GetUserTourDetailsAsync(
            int tourId)
        {
            using SqlConnection con =
                new SqlConnection(_connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "usp_GetUserTourDetails",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@TourId",
                SqlDbType.Int).Value = tourId;

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            // ========================================================
            // 1. CORE TOUR DETAILS
            // ========================================================

            if (!await reader.ReadAsync())
                return null;

            var dto = new UserTourDetailsDto
            {
                TourId = reader.GetInt32(0),

                TourTitle = reader.GetString(1),

                LocationName = reader.GetString(2),

                Overview = reader.IsDBNull(3)
                    ? null
                    : reader.GetString(3),

                Duration = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4),

                BasePrice = reader.GetDecimal(5),

                IncludedTotal = reader.GetDecimal(6),

                TotalPrice = reader.GetDecimal(7)
            };

            // ========================================================
            // 2. TOUR IMAGES
            // ========================================================

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    var imageUrl =
                        reader.GetString(0);

                    var isCover =
                        reader.GetBoolean(1);

                    dto.ImageUrls.Add(imageUrl);

                    if (isCover)
                    {
                        dto.CoverImageUrl = imageUrl;
                    }
                }
            }

            // ========================================================
            // 3. ADD-ONS
            // ========================================================

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dto.AddOns.Add(
                        reader.GetString(0));
                }
            }

            return dto;
        }
    }
}