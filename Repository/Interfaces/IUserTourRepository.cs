using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;

namespace Tour_Module.Repository.Interfaces
{
    public interface IUserTourRepository
    {
        Task<UserTourDetailsDto?> GetUserTourDetailsAsync(int tourId);
    }
}