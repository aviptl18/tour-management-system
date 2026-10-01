using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.DTOs.Option;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class TourAllowedOptionRepository : ITourAllowedOptionRepository
    {
        private readonly string _connectionString;

        public TourAllowedOptionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Read: Get allowed Add-Ons for a tour
        public async Task<List<TourAllowedOptionDto>> GetAllowedOptionsForTourAsync(int tourId)
        {
            var options = new List<TourAllowedOptionDto>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_GetAllowedOptionsForTour", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TourId", tourId);

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        options.Add(new TourAllowedOptionDto
                        {
                            OptionId = reader.GetInt32(reader.GetOrdinal("OptionId")),
                            OptionName = reader.GetString(reader.GetOrdinal("OptionName")),
                            BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                            Currency = reader.GetString(reader.GetOrdinal("Currency")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            IsDefault = reader.GetBoolean(reader.GetOrdinal("IsDefault"))
                        });
                    }
                }
            }

            return options;
        }

        // Get available optional addons

        public async Task<List<TourAllowedOptionDto>> GetAvailableOptionalOptionsForTourAsync(int tourId)
        {
            var options = new List<TourAllowedOptionDto>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_GetAvailableOptionalOptionsForTour", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TourId", tourId);

                await con.OpenAsync();

                using(SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        options.Add(new TourAllowedOptionDto
                        {
                            OptionId = reader.GetInt32(reader.GetOrdinal("OptionId")),
                            OptionName = reader.GetString(reader.GetOrdinal("OptionName")),
                            BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                            Currency = reader.GetString(reader.GetOrdinal("Currency")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            IsDefault = false
                        });
                    }
                }
            }
            return options;

        }

        // Admin: allow Add-On for tour
        public async Task AddAllowedOptionAsync(int tourId, int optionId, bool isDefault)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_AddAllowedOptionToTour", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TourId", tourId);
                cmd.Parameters.AddWithValue("@OptionId", optionId);
                cmd.Parameters.AddWithValue("@IsDefault", isDefault);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Admin: remove Add-On from tour
        public async Task RemoveAllowedOptionAsync(int tourId, int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_RemoveAllowedOptionFromTour", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TourId", tourId);
                cmd.Parameters.AddWithValue("@OptionId", optionId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
