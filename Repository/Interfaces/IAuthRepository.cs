using Tour_Module.Models.Entities;

namespace Tour_Module.Repository.Interfaces
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Gets user details required for login.
        /// </summary>
        Task<AuthUser?> GetUserForLoginAsync(string email);

        /// <summary>
        /// Gets all roles assigned to a user.
        /// </summary>
        Task<List<string>> GetUserRolesAsync(int userId);

        /// <summary>
        /// Checks whether an email already exists.
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        Task RegisterUserAsync(AuthUser user);
    }
}