using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class LocationImageRepository : ILocationImageRepository
    {
        private readonly IConfiguration _config;

        public LocationImageRepository(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
        }

        public async Task<List<LocationImage>> GetByLocationIdAsync(int locationId)
        {
            var list = new List<LocationImage>();

            using var con = GetConnection();
            using var cmd = new SqlCommand(
                "dbo.usp_Location_GetImages",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationId", SqlDbType.Int)
                          .Value = locationId;

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new LocationImage
                {
                    ImageId = reader.GetInt32(
                        reader.GetOrdinal("ImageId")),

                    LocationId = reader.GetInt32(
                        reader.GetOrdinal("LocationId")),

                    ImageUrl = reader.GetString(
                        reader.GetOrdinal("ImageUrl"))
                });
            }

            return list;
        }

        public async Task AddAsync(LocationImage locationImage)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand(
                "dbo.usp_Location_AddImage",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LocationId", SqlDbType.Int)
                          .Value = locationImage.LocationId;

            cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar, 500)
                          .Value = locationImage.ImageUrl;

            await con.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }
    }
}