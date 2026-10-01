using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class UserTourIndexService : IUserTourIndexService
    {
        private readonly ITourRepository _tourRepository;

        public UserTourIndexService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task<IEnumerable<TourDto>> GetActiveToursAsync()
        {
            // TEMPORARY reuse of existing repo
            // Later this can switch to:
            // usp_GetActiveToursForUser
            return await _tourRepository.GetAllToursAsync();
        }
    }
}
