using System.Security.Claims;
using Tour_Module.DTOs.Auth;
using System.Threading.Tasks;

namespace Tour_Module.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal?> AuthenticateAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);
    }
}
