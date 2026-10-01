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

        // Admin - Get all Option
        public async Task<List<Option>> GetAllAsync()
        {
            var options = new List<Option>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_GetAllOptions", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();

                using (SqlDataReader reader = await  cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        options.Add(MapOption(reader));
                    }
                }
            }
            return options;
        }

        // System - Get active Option
        public async Task<List<Option>> GetActiveAsync()
        {
            var options = new List<Option>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("GetActiveOptions", con))
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
        public async Task AddAsync(Option option)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_AddOption", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OptionName", option.OptionName);
                cmd.Parameters.AddWithValue("@BasePrice", option.BasePrice);
                cmd.Parameters.AddWithValue("@Currency", option.Currency);
                cmd.Parameters.AddWithValue("@Category", option.Category);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task UpdateAsync(Option option)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_UpdateOption", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OptionId", option.OptionId);
                cmd.Parameters.AddWithValue("@OptionName", option.OptionName);
                cmd.Parameters.AddWithValue("@BasePrice", option.BasePrice);
                cmd.Parameters.AddWithValue("@Currency", option.Currency);
                cmd.Parameters.AddWithValue("@Category", option.Category);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task SetStatusAsync(int optionId, bool isActive)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_SetOptionActiveStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OptionId", optionId);
                cmd.Parameters.AddWithValue("@IsActive", isActive);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            
        }

        public async Task<Option?> GetByIdAsync(int optionId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_GetOptionById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OptionId", optionId);

                await con.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                if(await reader.ReadAsync())
                    return MapOption(reader);

                return null;
            }
        }

        // CENTRAL MAPPER

        private Option MapOption(SqlDataReader reader)
        {
            return new Option
            {
                OptionId = reader.GetInt32(reader.GetOrdinal("OptionId")),
                OptionName = reader.GetString(reader.GetOrdinal("OptionName")),
                BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                Currency = reader.GetString(reader.GetOrdinal("Currency")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
    }
}

