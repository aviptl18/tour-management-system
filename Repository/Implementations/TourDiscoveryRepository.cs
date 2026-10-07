using System.Data;
using Microsoft.Data.SqlClient;
using Tour_Module.DTOs.TourDiscovery;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class TourDiscoveryRepository : ITourDiscoveryRepository
    {
        private readonly string _connectionString;

        public TourDiscoveryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<TourDiscoveryResultDto> SearchAsync(
            TourDiscoveryFilterDto filter)
        {
            var result = new TourDiscoveryResultDto();

            using SqlConnection con =
                new SqlConnection(_connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "usp_TourDiscovery_Search",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@SearchTerm",
                SqlDbType.NVarChar,
                100).Value =
                    (object?)filter.SearchTerm ?? DBNull.Value;

            cmd.Parameters.Add(
                "@LocationName",
                SqlDbType.NVarChar,
                100).Value =
                    (object?)filter.LocationName ?? DBNull.Value;

            cmd.Parameters.Add(
                "@MinPrice",
                SqlDbType.Decimal).Value =
                    (object?)filter.MinPrice ?? DBNull.Value;

            cmd.Parameters.Add(
                "@MaxPrice",
                SqlDbType.Decimal).Value =
                    (object?)filter.MaxPrice ?? DBNull.Value;

            cmd.Parameters.Add(
                "@SortBy",
                SqlDbType.VarChar,
                30).Value =
                    (object?)filter.SortBy ?? DBNull.Value;

            cmd.Parameters.Add(
                "@PageNumber",
                SqlDbType.Int).Value =
                    filter.PageNumber;

            cmd.Parameters.Add(
                "@PageSize",
                SqlDbType.Int).Value =
                    filter.PageSize;

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            // RESULT SET 1 — TOUR LIST
            while (await reader.ReadAsync())
            {
                result.Tours.Add(
                    new TourDiscoveryDto
                    {
                        TourId = reader.GetInt32(
                            reader.GetOrdinal("TourId")),

                        TourTitle = reader.GetString(
                            reader.GetOrdinal("TourTitle")),

                        LocationId = reader.GetInt32(
                            reader.GetOrdinal("LocationId")),

                        LocationName = reader.GetString(
                            reader.GetOrdinal("LocationName")),

                        Duration = reader.IsDBNull(
                            reader.GetOrdinal("Duration"))
                            ? null
                            : reader.GetString(
                                reader.GetOrdinal("Duration")),

                        BasePrice = reader.IsDBNull(
                            reader.GetOrdinal("BasePrice"))
                            ? null
                            : reader.GetDecimal(
                                reader.GetOrdinal("BasePrice")),

                        TotalPrice = reader.IsDBNull(
                            reader.GetOrdinal("TotalPrice"))
                            ? null
                            : reader.GetDecimal(
                                reader.GetOrdinal("TotalPrice")),

                        CoverImageUrl = reader.IsDBNull(
                            reader.GetOrdinal("CoverImageUrl"))
                            ? null
                            : reader.GetString(
                                reader.GetOrdinal("CoverImageUrl"))
                    });
            }

            // RESULT SET 2 — TOTAL COUNT
            if (await reader.NextResultAsync())
            {
                if (await reader.ReadAsync())
                {
                    result.TotalCount =
                        reader.GetInt32(
                            reader.GetOrdinal("TotalCount"));
                }
            }

            return result;
        }

        public async Task<TourDetailsDto?> GetDetailsAsync(
            int tourId)
        {
            using SqlConnection con =
                new SqlConnection(_connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "usp_TourDiscovery_GetDetails",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@TourId",
                SqlDbType.Int).Value =
                    tourId;

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            // RESULT SET 1 — TOUR DETAILS
            if (!await reader.ReadAsync())
                return null;

            var tour = new TourDetailsDto
            {
                TourId = reader.GetInt32(
                    reader.GetOrdinal("TourId")),

                TourTitle = reader.GetString(
                    reader.GetOrdinal("TourTitle")),

                LocationName = reader.GetString(
                    reader.GetOrdinal("LocationName")),

                Overview = reader.IsDBNull(
                    reader.GetOrdinal("Overview"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Overview")),

                Duration = reader.IsDBNull(
                    reader.GetOrdinal("Duration"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Duration")),

                BasePrice = reader.IsDBNull(
                    reader.GetOrdinal("BasePrice"))
                    ? 0
                    : reader.GetDecimal(
                        reader.GetOrdinal("BasePrice")),

                IncludedTotal = reader.IsDBNull(
                    reader.GetOrdinal("IncludedTotal"))
                    ? 0
                    : reader.GetDecimal(
                        reader.GetOrdinal("IncludedTotal")),

                TotalPrice = reader.IsDBNull(
                    reader.GetOrdinal("TotalPrice"))
                    ? 0
                    : reader.GetDecimal(
                        reader.GetOrdinal("TotalPrice"))
            };

            // RESULT SET 2 — IMAGES
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(
                        reader.GetOrdinal("ImageUrl")))
                    {
                        tour.ImageUrls.Add(
                            reader.GetString(
                                reader.GetOrdinal("ImageUrl")));
                    }
                }
            }

            // RESULT SET 3 — ADD-ONS
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(
                        reader.GetOrdinal("OptionName")))
                    {
                        tour.AddOns.Add(
                            reader.GetString(
                                reader.GetOrdinal("OptionName")));
                    }
                }
            }

            return tour;
        }
    }
}