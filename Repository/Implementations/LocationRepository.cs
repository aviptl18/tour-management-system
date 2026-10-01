using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

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
            using var cmd = new SqlCommand("usp_GetAllLocations", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IncludeInactive", includeInactive);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Location
                {
                    LocationId = (int)reader["LocationId"],
                    LocationName = reader["LocationName"].ToString()!,
                    IsActive = (bool)reader["IsActive"],
                    CreatedDate = (DateTime)reader["CreatedDate"]
                });
            }
            return list;

        }
        public async Task<Location?> GetByIdAsync(int locationId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_GetAllLocations", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IncludeInactive", 1);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                if ((int)reader["LocationId"] == locationId)
                {
                    return new Location
                    {
                        LocationId = (int)reader["LocationId"],
                        LocationName = reader["LocationName"].ToString()!,
                        IsActive = (bool)reader["IsActive"],
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    };
                }
            }
            return null;

        }

        public async Task InsertAsync(string locationName)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_AddLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationName", locationName);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

        }
        public async Task UpdateAsync(int locationId, string locationName, bool isActive)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_UpdateLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationId", locationId);
            cmd.Parameters.AddWithValue("@LocationName", locationName);
            cmd.Parameters.AddWithValue("IsActive", isActive);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

        }
        public async Task SetInactiveAsync(int locationId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_DeactivateLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationId", locationId);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

        }

       public async Task<List<Location>> SearchLocationsAsync(string? searchTerm)
        {
            var list = new List<Location>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_SearchLocations", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@SearchTerm",
                string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm
            );

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Location
                {
                    LocationId = (int)reader["LocationId"],
                    LocationName = reader["LocationName"].ToString()!
                });
            }

            return list;
        }
    }
}
