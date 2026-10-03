using Tour_Module.Models.Entities;

namespace Tour_Module.Repository.Interfaces
{
    public interface IOptionRepository
    {
        Task<List<Option>> GetAllAsync();

        Task<List<Option>> GetActiveAsync();

        Task<Option?> GetByIdAsync(int optionId);

        Task CreateAsync(Option option);

        Task UpdateAsync(Option option);

        Task DeactivateAsync(int optionId);

        Task ActivateAsync(int optionId);
    }
}