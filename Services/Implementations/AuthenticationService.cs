using System.Security.Claims;
using Tour_Module.DTOs.Authentication;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Results;
using Tour_Module.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Tour_Module.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IPasswordHasher passwordHasher,
            ILogger<AuthenticationService> logger)
        {
            _authenticationRepository = authenticationRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(
            LoginRequestDto request)
        {
            string email =
                request.Email.Trim().ToLowerInvariant();

            AuthenticationUser? user =
                await _authenticationRepository
                    .LoginUserAsync(email);

            if (user == null)
            {
                _logger.LogWarning(
                    "Login failed because the supplied credentials were invalid for email {Email}.",
                    email);
                return new AuthenticationResult
                {
                    InvalidCredentials = true
                };
            }

            if (!user.IsActive)
            {
                _logger.LogWarning(
                   "Login attempt rejected because user {UserId} with email {Email} is inactive.",
                   user.UserId,
                   user.Email);

                return new AuthenticationResult
                {
                    Inactive = true,
                    UserId = user.UserId
                };
            }

            if (user.LockoutEnd.HasValue &&
                user.LockoutEnd.Value > DateTime.UtcNow)
            {
                _logger.LogWarning(
                    "Login attempt rejected because user {UserId} with email {Email} is locked out.",
                    user.UserId,
                    user.Email);

                return new AuthenticationResult
                {
                    LockedOut = true,
                    UserId = user.UserId
                };
            }

            bool isPasswordValid =
                _passwordHasher.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!isPasswordValid)
            {
                await _authenticationRepository
                    .RecordFailedLoginAsync(user.UserId);

                _logger.LogWarning(
                    "Login failed for user {UserId} with email {Email}.",
                    user.UserId,
                    user.Email);

                return new AuthenticationResult
                {
                    InvalidCredentials = true,
                    UserId = user.UserId
                };
            }

            await _authenticationRepository
                .ResetLoginAttemptsAsync(user.UserId);

            List<string> roles =
                await _authenticationRepository
                    .GetUserRolesAsync(user.UserId);

            List<Claim> claims =
            [
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.FullName),

                new Claim(
                    ClaimTypes.Email,
                    user.Email)
            ];

            foreach (string role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }

            ClaimsIdentity identity =
                new ClaimsIdentity(
                    claims,
                    "Cookies");

            ClaimsPrincipal principal =
                new ClaimsPrincipal(identity);

            _logger.LogInformation(
                "User {UserId} with email {Email} logged in successfully.",
                user.UserId,
                user.Email);

            return new AuthenticationResult
            {
                Succeeded = true,
                Principal = principal,
                UserId = user.UserId,
                Roles = roles
            };
        }

        public async Task<AuthenticationResult> RegisterAsync(
            RegisterRequestDto request)
        {
            string email =
                request.Email.Trim().ToLowerInvariant();

            AuthenticationUser user = new()
            {
                FullName = request.FullName.Trim(),
                Email = email,
                PasswordHash =
                    _passwordHasher.Hash(request.Password)
            };

            int userId =
                await _authenticationRepository
                    .RegisterUserAsync(user);

            _logger.LogInformation(
                "User {UserId} with email {Email} registered successfully.",
                userId,
                user.Email);

            List<string> roles =
                await _authenticationRepository
                    .GetUserRolesAsync(userId);

            List<Claim> claims =
            [
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.FullName),

                new Claim(
                    ClaimTypes.Email,
                    user.Email)
            ];

            foreach (string role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }

            ClaimsIdentity identity =
                new ClaimsIdentity(
                    claims,
                    "Cookies");

            ClaimsPrincipal principal =
                new ClaimsPrincipal(identity);

            return new AuthenticationResult
            {
                Succeeded = true,
                Principal = principal,
                UserId = userId,
                Roles = roles
            };
        }
    }
}