using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class OptionRepository : IOptionRepository
    {
        private readonly string _connectionString;

        public OptionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Option>> GetAllAsync()
        {
            var options = new List<Option>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_GetAll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        options.Add(MapOption(reader));
                    }
                }
            }

            return options;
        }

        public async Task<List<Option>> GetActiveAsync()
        {
            var options = new List<Option>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_GetActive", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        options.Add(MapOption(reader));
                    }
                }
            }

            return options;
        }

        public async Task<Option?> GetByIdAsync(int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_GetById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OptionId", SqlDbType.Int).Value = optionId;

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapOption(reader);
                    }
                }
            }

            return null;
        }

        public async Task CreateAsync(Option option)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_Create", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OptionName", SqlDbType.NVarChar, 150)
                    .Value = option.OptionName;

                var basePriceParameter = cmd.Parameters.Add("@BasePrice", SqlDbType.Decimal);
                basePriceParameter.Precision = 10;
                basePriceParameter.Scale = 2;
                basePriceParameter.Value = option.BasePrice;

                cmd.Parameters.Add("@Currency", SqlDbType.NVarChar, 10)
                    .Value = option.Currency;

                cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 100)
                    .Value = option.Category;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Option option)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_Update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OptionId", SqlDbType.Int)
                    .Value = option.OptionId;

                cmd.Parameters.Add("@OptionName", SqlDbType.NVarChar, 150)
                    .Value = option.OptionName;

                var basePriceParameter = cmd.Parameters.Add("@BasePrice", SqlDbType.Decimal);
                basePriceParameter.Precision = 10;
                basePriceParameter.Scale = 2;
                basePriceParameter.Value = option.BasePrice;

                cmd.Parameters.Add("@Currency", SqlDbType.NVarChar, 10)
                    .Value = option.Currency;

                cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 100)
                    .Value = option.Category;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeactivateAsync(int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_Deactivate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OptionId", SqlDbType.Int)
                    .Value = optionId;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task ActivateAsync(int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_Option_Activate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OptionId", SqlDbType.Int)
                    .Value = optionId;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private Option MapOption(SqlDataReader reader)
        {
            return new Option
            {
                OptionId = reader.GetInt32(reader.GetOrdinal("OptionId")),
                OptionName = reader.GetString(reader.GetOrdinal("OptionName")),
                BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                Currency = reader.GetString(reader.GetOrdinal("Currency")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }
    }
}