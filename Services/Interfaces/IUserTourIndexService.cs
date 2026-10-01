using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;

namespace Tour_Module.Services.Interfaces
{
    /// <summary>
    /// User-facing tour listing service.
    /// Responsible ONLY for browsing tours.
    /// </summary>
    public interface IUserTourIndexService
    {
        Task<IEnumerable<TourDto>> GetActiveToursAsync();
    }
}
