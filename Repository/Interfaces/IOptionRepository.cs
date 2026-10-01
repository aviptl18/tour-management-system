using Tour_Module.Models.Entities;

namespace Tour_Module.Repository.Interfaces
{
    public interface IOptionRepository
    {
        Task<List<Option>> GetAllAsync();
        Task<List<Option>> GetActiveAsync();
        Task AddAsync(Option option);
        Task UpdateAsync(Option option);
        Task SetStatusAsync(int  optionId, bool isActive);
        Task<Option?> GetByIdAsync(int optionId);
    }
}
