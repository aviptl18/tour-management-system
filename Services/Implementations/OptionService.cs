using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;

        public OptionService(IOptionRepository optionRepository)
        {
            _optionRepository = optionRepository;
        }

        public async Task<List<Option>> GetAllOptionsAsync()
        {
            return await _optionRepository.GetAllAsync();
        }

        public async Task<List<Option>> GetActiveOptionsAsync()
        {
            return await _optionRepository.GetActiveAsync();
        }

        public async Task<Option?> GetOptionByIdAsync(int optionId)
        {
            return await _optionRepository.GetByIdAsync(optionId);
        }

        public async Task CreateOptionAsync(Option option)
        {
            if (option.BasePrice <= 0)
                throw new ArgumentException("Base price must be greater than zero.");

            await _optionRepository.CreateAsync(option);
        }

        public async Task UpdateOptionAsync(Option option)
        {
            if (option.BasePrice <= 0)
                throw new ArgumentException("Base price must be greater than zero.");

            await _optionRepository.UpdateAsync(option);
        }

        public async Task DeactivateOptionAsync(int optionId)
        {
            await _optionRepository.DeactivateAsync(optionId);
        }

        public async Task ActivateOptionAsync(int optionId)
        {
            await _optionRepository.ActivateAsync(optionId);
        }
    }
}