using System.Security.Claims;
using Tour_Module.DTOs.Authentication;
using Tour_Module.Results;

namespace Tour_Module.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(
            LoginRequestDto request);

        Task<AuthenticationResult> RegisterAsync(
            RegisterRequestDto request);
    }
}