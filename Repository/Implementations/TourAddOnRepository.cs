using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.DTOs.Option;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class TourAddOnRepository : ITourAddOnRepository
    {
        private readonly string _connectionString;

        public TourAddOnRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<TourAddOnDto>> GetConfigurationAsync(int tourId)
        {
            var addOns = new List<TourAddOnDto>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourAddOn_GetConfiguration", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        addOns.Add(new TourAddOnDto
                        {
                            OptionId = reader.GetInt32(
                                reader.GetOrdinal("OptionId")),

                            OptionName = reader.GetString(
                                reader.GetOrdinal("OptionName")),

                            BasePrice = reader.GetDecimal(
                                reader.GetOrdinal("BasePrice")),

                            Currency = reader.GetString(
                                reader.GetOrdinal("Currency")),

                            Category = reader.GetString(
                                reader.GetOrdinal("Category")),

                            IsDefault = reader.GetBoolean(
                                reader.GetOrdinal("IsDefault"))
                        });
                    }
                }
            }

            return addOns;
        }

        public async Task<List<TourAddOnDto>> GetAvailableAddOnsAsync(int tourId)
        {
            var addOns = new List<TourAddOnDto>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourAddOn_GetAvailable", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        addOns.Add(new TourAddOnDto
                        {
                            OptionId = reader.GetInt32(
                                reader.GetOrdinal("OptionId")),

                            OptionName = reader.GetString(
                                reader.GetOrdinal("OptionName")),

                            BasePrice = reader.GetDecimal(
                                reader.GetOrdinal("BasePrice")),

                            Currency = reader.GetString(
                                reader.GetOrdinal("Currency")),

                            Category = reader.GetString(
                                reader.GetOrdinal("Category")),

                            IsDefault = false
                        });
                    }
                }
            }

            return addOns;
        }

        public async Task AddOptionalAddOnAsync(int tourId, int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourAddOn_Add", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                cmd.Parameters.Add(
                    "@OptionId",
                    SqlDbType.Int).Value = optionId;

                

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task RemoveOptionalAddOnAsync(int tourId, int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "usp_TourAddOn_Remove", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@TourId",
                    SqlDbType.Int).Value = tourId;

                cmd.Parameters.Add(
                    "@OptionId",
                    SqlDbType.Int).Value = optionId;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}