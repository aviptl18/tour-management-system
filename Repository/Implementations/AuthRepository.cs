using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ============================================================
        // LOGIN
        // ============================================================

        public async Task<AuthUser?> GetUserForLoginAsync(string email)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_LoginUser", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapAuthUser(reader);
        }

        // ============================================================
        // USER ROLES
        // ============================================================

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            List<string> roles = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_GetUserRoles", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(reader.GetString(reader.GetOrdinal("RoleName")));
            }

            return roles;
        }

        // ============================================================
        // EMAIL EXISTS
        // ============================================================

        public async Task<bool> EmailExistsAsync(string email)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_GetUserByEmail", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            return await reader.ReadAsync();
        }

        // ============================================================
        // REGISTER USER
        // ============================================================

        public async Task RegisterUserAsync(AuthUser user)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_RegisterUser", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

            // SQL Server automatically assigns the "User" role
            // because @RoleName has a default value.

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================================
        // CENTRAL MAPPER
        // ============================================================

        private static AuthUser MapAuthUser(SqlDataReader reader)
        {
            return new AuthUser
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
            };
        }
    }
}