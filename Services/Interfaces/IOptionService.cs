using Tour_Module.DTOs.Option;
using Tour_Module.Models.Entities;

namespace Tour_Module.Services.Interfaces
{
    public interface IOptionService
    {
        Task<List<Option>> GetAllOptionsAsync();
        Task<List<Option>> GetActiveOptionsAsync();
        Task AddOptionAsync(Option option);
        Task UpdateOptionAsync(Option option);
        Task SetOptionStatusAsync(int optionId, bool isActive);
    }

}
