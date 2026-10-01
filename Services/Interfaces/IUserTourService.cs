using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;

namespace Tour_Module.Services.Interfaces
{
    public interface IUserTourService
    {
        Task<UserTourDetailsDto?> GetUserTourDetailsAsync(int tourId);
    }
}
