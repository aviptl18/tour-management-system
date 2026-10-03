using Tour_Module.Models.Entities;

namespace Tour_Module.Services.Interfaces
{
    public interface IOptionService
    {
        Task<List<Option>> GetAllOptionsAsync();
        Task<List<Option>> GetActiveOptionsAsync();
        Task<Option?> GetOptionByIdAsync(int optionId);
        Task CreateOptionAsync(Option option);
        Task UpdateOptionAsync(Option option);
        Task DeactivateOptionAsync(int optionId);
        Task ActivateOptionAsync(int optionId);
    }
}