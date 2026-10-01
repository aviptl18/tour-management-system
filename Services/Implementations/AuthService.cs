using System.Security.Claims;
using BCrypt.Net;
using Tour_Module.DTOs.Auth;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        // ============================================================
        // LOGIN
        // ============================================================

        public async Task<ClaimsPrincipal?> AuthenticateAsync(LoginDto dto)
        {
            // Get user by email
            AuthUser? user = await _authRepository.GetUserForLoginAsync(dto.Email);

            if (user == null)
                return null;

            // Verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return null;

            // Get user roles
            List<string> roles = await _authRepository.GetUserRolesAsync(user.UserId);

            // Build Claims
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            ];

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                "Cookies"
            );

            return new ClaimsPrincipal(identity);
        }

        // ============================================================
        // REGISTER
        // ============================================================

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            bool emailExists = await _authRepository.EmailExistsAsync(dto.Email);

            if (emailExists)
                return false;

            AuthUser user = new AuthUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _authRepository.RegisterUserAsync(user);

            return true;
        }
    }
}