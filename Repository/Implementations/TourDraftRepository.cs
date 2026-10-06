using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class TourDraftRepository : ITourDraftRepository
    {
        private readonly string _connectionString;

        public TourDraftRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // =========================================================
        // DRAFT LIFECYCLE
        // =========================================================

        public async Task<int> CreateDraftAsync(int adminId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_Create", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CreatedByAdminId",
                    SqlDbType.Int).Value = adminId;

                SqlParameter draftIdParameter = cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int);

                draftIdParameter.Direction =
                    ParameterDirection.Output;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                return Convert.ToInt32(draftIdParameter.Value);
            }
        }

        public async Task<TourDraft?> GetDraftByIdAsync(
            int draftId,
            int adminId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_GetById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@CreatedByAdminId",
                    SqlDbType.Int).Value = adminId;

                await con.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new TourDraft
                        {
                            DraftId = reader.GetInt32(
                                reader.GetOrdinal("DraftId")),

                            TourTitle = reader.GetString(
                                reader.GetOrdinal("TourTitle")),

                            Overview = reader.GetString(
                                reader.GetOrdinal("Overview")),

                            LocationId = reader.IsDBNull(
                                reader.GetOrdinal("LocationId"))
                                ? null
                                : reader.GetInt32(
                                    reader.GetOrdinal("LocationId")),

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

                            DraftStep = reader.GetInt32(
                                reader.GetOrdinal("DraftStep")),

                            Status = reader.GetString(
                                reader.GetOrdinal("Status")),

                            OriginalTourId = reader.IsDBNull(
                                reader.GetOrdinal("OriginalTourId"))
                                ? null
                                : reader.GetInt32(
                                    reader.GetOrdinal("OriginalTourId")),

                            CreatedByAdminId = reader.GetInt32(
                                reader.GetOrdinal("CreatedByAdminId")),

                            CreatedAt = reader.GetDateTime(
                                reader.GetOrdinal("CreatedAt")),

                            UpdatedAt = reader.IsDBNull(
                                reader.GetOrdinal("UpdatedAt"))
                                ? null
                                : reader.GetDateTime(
                                    reader.GetOrdinal("UpdatedAt"))
                        };
                    }
                }
            }

            return null;
        }

        // =========================================================
        // STEP 1 - BASIC INFORMATION
        // =========================================================

        public async Task UpdateDraftBasicInfoAsync(
            int draftId,
            int adminId,
            string title,
            string overview)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_UpdateBasicInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@CreatedByAdminId",
                    SqlDbType.Int).Value = adminId;

                cmd.Parameters.Add(
                    "@TourTitle",
                    SqlDbType.NVarChar, 200).Value = title;

                cmd.Parameters.Add(
                    "@Overview",
                    SqlDbType.NVarChar, -1).Value = overview;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        // =========================================================
        // STEP 2 - LOCATION
        // =========================================================

        public async Task UpdateDraftLocationAsync(
            int draftId,
            int adminId,
            int locationId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_UpdateLocation", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                cmd.Parameters.Add(
                    "@LocationId",
                    SqlDbType.Int).Value = locationId;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        // =========================================================
        // STEP 2 - IMAGES
        // =========================================================

        public async Task DeleteAllDraftImagesAsync(
            int draftId,
            int adminId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftImage_DeleteAll", con))
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

        public async Task AddDraftImageAsync(
            int draftId,
            int adminId,
            int locationImageId,
            bool isCover)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftImage_Add", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                cmd.Parameters.Add(
                    "@LocationImageId",
                    SqlDbType.Int).Value = locationImageId;

                cmd.Parameters.Add(
                    "@IsCover",
                    SqlDbType.Bit).Value = isCover;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<TourDraftImage>> GetDraftImagesAsync(
            int draftId,
            int adminId)
        {
            var images = new List<TourDraftImage>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftImage_Get", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                await con.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        images.Add(new TourDraftImage
                        {
                            DraftImageId = reader.GetInt32(
                                reader.GetOrdinal("DraftImageId")),

                            DraftId = reader.GetInt32(
                                reader.GetOrdinal("DraftId")),

                            LocationImageId = reader.GetInt32(
                                reader.GetOrdinal("LocationImageId")),

                            IsCover = reader.GetBoolean(
                                reader.GetOrdinal("IsCover")),

                            CreatedAt = reader.GetDateTime(
                                reader.GetOrdinal("CreatedAt"))
                        });
                    }
                }
            }

            return images;
        }

        // =========================================================
        // STEP 3 - PRICING
        // =========================================================

        public async Task EnsureBasePricingComponentAsync(
            int draftId,
            int adminId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftPricing_EnsureBase", con))
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

        public async Task<List<TourDraftPricingComponent>>
            GetDraftPricingComponentsAsync(
                int draftId,
                int adminId)
        {
            var components =
                new List<TourDraftPricingComponent>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftPricing_Get", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                await con.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        components.Add(
                            new TourDraftPricingComponent
                            {
                                PricingComponentId =
                                    reader.GetInt32(
                                        reader.GetOrdinal(
                                            "PricingComponentId")),

                                DraftId =
                                    reader.GetInt32(
                                        reader.GetOrdinal(
                                            "DraftId")),

                                OptionId =
                                    reader.IsDBNull(
                                        reader.GetOrdinal(
                                            "OptionId"))
                                    ? null
                                    : reader.GetInt32(
                                        reader.GetOrdinal(
                                            "OptionId")),

                                ComponentName =
                                    reader.GetString(
                                        reader.GetOrdinal(
                                            "ComponentName")),

                                Price =
                                    reader.GetDecimal(
                                        reader.GetOrdinal(
                                            "Price")),

                                IsBaseComponent =
                                    reader.GetBoolean(
                                        reader.GetOrdinal(
                                            "IsBaseComponent")),

                                CreatedAt =
                                    reader.GetDateTime(
                                        reader.GetOrdinal(
                                            "CreatedAt"))
                            });
                    }
                }
            }

            return components;
        }

        public async Task AddDraftPricingComponentAsync(
            int draftId,
            int adminId,
            int optionId,
            string componentName,
            decimal price)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftPricing_Add", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                cmd.Parameters.Add(
                    "@OptionId",
                    SqlDbType.Int).Value = optionId;

                cmd.Parameters.Add(
                    "@ComponentName",
                    SqlDbType.NVarChar, 150).Value =
                    componentName;

                var priceParameter = cmd.Parameters.Add(
                    "@Price",
                    SqlDbType.Decimal);

                priceParameter.Precision = 10;
                priceParameter.Scale = 2;
                priceParameter.Value = price;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task RemoveDraftPricingComponentAsync(
            int pricingComponentId,
            int adminId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraftPricing_Remove", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@PricingComponentId",
                    SqlDbType.Int).Value =
                    pricingComponentId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateDraftBasePriceAsync(
            int draftId,
            int adminId,
            int pricingComponentId,
            decimal price)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            using (SqlCommand cmd =
                new SqlCommand(
                    "usp_TourDraftPricing_UpdateBasePrice",
                    con))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value =
                    draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value =
                    adminId;

                cmd.Parameters.Add(
                    "@ComponentId",
                    SqlDbType.Int).Value =
                    pricingComponentId;

                var priceParameter = cmd.Parameters.Add(
                    "@Price",
                    SqlDbType.Decimal);

                priceParameter.Precision = 10;
                priceParameter.Scale = 2;
                priceParameter.Value = price;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        // =========================================================
        // DRAFT PROGRESS
        // =========================================================

        public async Task SetDraftStepAsync(
            int draftId,
            int adminId,
            int step)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_SetStep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                cmd.Parameters.Add(
                    "@Step",
                    SqlDbType.Int).Value = step;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateDraftDurationAsync(
            int draftId,
            int adminId,
            string duration)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_UpdateDuration", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@AdminId",
                    SqlDbType.Int).Value = adminId;

                cmd.Parameters.Add(
                    "@Duration",
                    SqlDbType.NVarChar, 100).Value =
                    duration;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        // =========================================================
        // DRAFT STATUS
        // =========================================================

        public async Task UpdateDraftStatusAsync(
            int draftId,
            string status)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourDraft_UpdateStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@DraftId",
                    SqlDbType.Int).Value = draftId;

                cmd.Parameters.Add(
                    "@Status",
                    SqlDbType.NVarChar, 20).Value = status;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        // =========================================================
        // EDIT EXISTING TOUR
        // =========================================================

        public async Task<int?> GetActiveDraftForTourAsync(
            int tourId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
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

                if (result == null || result == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(result);
            }
        }

        public async Task<int> CreateDraftFromTourAsync(
            int tourId,
            int adminId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
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

                SqlParameter draftIdParameter = cmd.Parameters.Add(
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
    }
}