using Tour_Module.Models.Entities;

namespace Tour_Module.Repository.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticationUser?> LoginUserAsync(string email);

        Task<List<string>> GetUserRolesAsync(int userId);

        Task<int> RegisterUserAsync(AuthenticationUser user);

        Task RecordFailedLoginAsync(int userId);

        Task ResetLoginAttemptsAsync(int userId);
    }
}