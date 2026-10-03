using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Tour_Module.Repository.Implementations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IConfiguration _config;

        public LocationRepository(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }


        public async Task<List<Location>> GetAllAsync(bool includeInactive)
        {
            var list = new List<Location>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_GetAll", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@IncludeInactive", SqlDbType.Bit)
                          .Value = includeInactive;

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Location
                {
                    LocationId = reader.GetInt32(reader.GetOrdinal("LocationId")),
                    LocationName = reader.GetString(reader.GetOrdinal("LocationName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                });
            }

            return list;
        }
        public async Task<Location?> GetByIdAsync(int locationId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_GetById", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationId", SqlDbType.Int)
                          .Value = locationId;

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Location
                {
                    LocationId = reader.GetInt32(reader.GetOrdinal("LocationId")),
                    LocationName = reader.GetString(reader.GetOrdinal("LocationName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
            }

            return null;
        }

        public async Task AddAsync(string locationName)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_Create", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationName", SqlDbType.NVarChar, 200)
                          .Value = locationName;

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(int locationId, string locationName)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_Update", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationId", SqlDbType.Int)
                          .Value = locationId;

            cmd.Parameters.Add("@LocationName", SqlDbType.NVarChar, 200)
                          .Value = locationName;

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeactivateAsync(int locationId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_Deactivate", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationId", SqlDbType.Int)
                          .Value = locationId;

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActivateAsync(int locationId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_Activate",con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationId", SqlDbType.Int)
                          .Value = locationId;

            await con.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Location>> SearchAsync(string? searchTerm)
        {
            var list = new List<Location>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_Location_Search", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 200)
                          .Value = string.IsNullOrWhiteSpace(searchTerm)
                              ? DBNull.Value
                              : searchTerm.Trim();

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Location
                {
                    LocationId = reader.GetInt32(reader.GetOrdinal("LocationId")),
                    LocationName = reader.GetString(reader.GetOrdinal("LocationName"))
                });
            }

            return list;
        }
    }
}
