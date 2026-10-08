using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly string _connectionString;

        public AuthenticationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ============================================================
        // LOGIN USER
        // ============================================================

        public async Task<AuthenticationUser?> LoginUserAsync(string email)
        {
            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(
                "usp_Authentication_LoginUser",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@Email",
                SqlDbType.NVarChar,
                150).Value = email;

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapAuthenticationUser(reader);
        }

        // ============================================================
        // GET USER ROLES
        // ============================================================

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            List<string> roles = new();

            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(
                "usp_Authentication_GetUserRoles",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@UserId",
                SqlDbType.Int).Value = userId;

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(
                    reader.GetString(
                        reader.GetOrdinal("RoleName")));
            }

            return roles;
        }

        // ============================================================
        // REGISTER USER
        // ============================================================

        public async Task<int> RegisterUserAsync(
            AuthenticationUser user)
        {
            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(
                "usp_Authentication_RegisterUser",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@FullName",
                SqlDbType.NVarChar,
                150).Value = user.FullName;

            cmd.Parameters.Add(
                "@Email",
                SqlDbType.NVarChar,
                150).Value = user.Email;

            cmd.Parameters.Add(
                "@PasswordHash",
                SqlDbType.NVarChar,
                500).Value = user.PasswordHash;

            await con.OpenAsync();

            object? result = await cmd.ExecuteScalarAsync();

            if (result == null ||
                result == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "User registration did not return a UserId.");
            }

            return Convert.ToInt32(result);
        }

        // ============================================================
        // RECORD FAILED LOGIN
        // ============================================================

        public async Task RecordFailedLoginAsync(int userId)
        {
            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(
                "usp_Authentication_RecordFailedLogin",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@UserId",
                SqlDbType.Int).Value = userId;

            await con.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================================
        // RESET LOGIN ATTEMPTS
        // ============================================================

        public async Task ResetLoginAttemptsAsync(int userId)
        {
            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(
                "usp_Authentication_ResetLoginAttempts",
                con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@UserId",
                SqlDbType.Int).Value = userId;

            await con.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        // ============================================================
        // AUTHENTICATION USER MAPPER
        // ============================================================

        private static AuthenticationUser MapAuthenticationUser(
            SqlDataReader reader)
        {
            return new AuthenticationUser
            {
                UserId = reader.GetInt32(
                    reader.GetOrdinal("UserId")),

                FullName = reader.GetString(
                    reader.GetOrdinal("FullName")),

                Email = reader.GetString(
                    reader.GetOrdinal("Email")),

                PasswordHash = reader.GetString(
                    reader.GetOrdinal("PasswordHash")),

                IsActive = reader.GetBoolean(
                    reader.GetOrdinal("IsActive")),

                FailedLoginAttempts = reader.GetInt32(
                    reader.GetOrdinal("FailedLoginAttempts")),

                LockoutEnd = reader.IsDBNull(
                    reader.GetOrdinal("LockoutEnd"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("LockoutEnd"))
            };
        }
    }
}