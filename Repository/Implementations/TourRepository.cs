using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Repository.Implementations
{
    public class TourRepository : ITourRepository
    {
        private readonly string _connectionString;

        public TourRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // =========================================================
        // READ - GET ALL TOURS
        // =========================================================

        public async Task<List<TourDto>> GetAllAsync()
        {
            var tours = new List<TourDto>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_Tour_GetAll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tours.Add(new TourDto
                        {
                            TourId = reader.GetInt32(
                                reader.GetOrdinal("TourId")),

                            LocationId = reader.GetInt32(
                                reader.GetOrdinal("LocationId")),

                            LocationName = reader.GetString(
                                reader.GetOrdinal("LocationName")),

                            TourTitle = reader.GetString(
                                reader.GetOrdinal("TourTitle")),

                            Duration = reader.IsDBNull(
                                reader.GetOrdinal("Duration"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("Duration")),

                            Overview = reader.IsDBNull(
                                reader.GetOrdinal("Overview"))
                                ? null
                                : reader.GetString(
                                    reader.GetOrdinal("Overview")),

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
                }
            }

            return tours;
        }

        // =========================================================
        // READ - GET TOUR DETAILS
        // =========================================================

        public async Task<TourDetailsViewModel?> GetDetailsAsync(
            int tourId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_Tour_GetById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                await con.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    // =====================================================
                    // RESULT SET 1 — CORE TOUR DETAILS
                    // =====================================================

                    if (!await reader.ReadAsync())
                        return null;

                    var tour = new TourDetailsViewModel
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
                                reader.GetOrdinal("TotalPrice")),

                        CoverImageUrl = reader.IsDBNull(
                            reader.GetOrdinal("CoverImageUrl"))
                            ? null
                            : reader.GetString(
                                reader.GetOrdinal("CoverImageUrl"))
                    };


                    // =====================================================
                    // RESULT SET 2 — ALL TOUR IMAGES
                    // =====================================================

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


                    // =====================================================
                    // RESULT SET 3 — ALLOWED ADD-ONS
                    // =====================================================

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

        // =========================================================
        // PUBLISH - CREATE LIVE TOUR FROM COMPLETED DRAFT
        // =========================================================

        public async Task<int> PublishFromDraftAsync(
            int draftId,
            int adminId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_Tour_Publish", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                SqlParameter newTourIdParameter =
                    cmd.Parameters.Add(
                        "@NewTourId",
                        SqlDbType.Int);

                newTourIdParameter.Direction =
                    ParameterDirection.Output;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                return Convert.ToInt32(
                    newTourIdParameter.Value);
            }
        }

        // =========================================================
        // EDIT - GET ACTIVE DRAFT ID FOR TOUR
        // =========================================================

        public async Task<int?> GetActiveDraftIdAsync(
            int tourId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_GetActiveForTour", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                await con.OpenAsync();

                object? result =
                    await cmd.ExecuteScalarAsync();

                if (result == null ||
                    result == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(result);
            }
        }

        // =========================================================
        // EDIT - CREATE DRAFT FROM EXISTING TOUR
        // =========================================================

        public async Task<int> CreateDraftFromTourAsync(
            int tourId,
            int adminId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_CreateFromTour", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                SqlParameter draftIdParameter =
                    cmd.Parameters.Add(
                        "@DraftId",
                        SqlDbType.Int);

                draftIdParameter.Direction =
                    ParameterDirection.Output;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                return Convert.ToInt32(
                    draftIdParameter.Value);
            }
        }

        // =========================================================
        // EDIT - UPDATE LIVE TOUR FROM COMPLETED DRAFT
        // =========================================================

        public async Task UpdateFromDraftAsync(
            int draftId,
            int adminId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_Tour_UpdateFromDraft", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int tourId)
        {
            using SqlConnection con =
                new SqlConnection(_connectionString);

            using SqlCommand cmd =
                new SqlCommand(
                    "usp_Tour_Delete",
                    con);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@TourId",
                SqlDbType.Int).Value = tourId;

            await con.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }
    }
}