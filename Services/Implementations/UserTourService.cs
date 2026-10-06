using System;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class UserTourService : IUserTourService
    {
        private readonly IUserTourRepository _userTourRepository;

        public UserTourService(
            IUserTourRepository userTourRepository)
        {
            _userTourRepository = userTourRepository
                ?? throw new ArgumentNullException(
                    nameof(userTourRepository));
        }

        public async Task<UserTourDetailsDto?> GetUserTourDetailsAsync(
            int tourId)
        {
            if (tourId <= 0)
                return null;

            return await _userTourRepository
                .GetUserTourDetailsAsync(tourId);
        }
    }
}