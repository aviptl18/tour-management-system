using Tour_Module.DTOs.Option;
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

        public async Task AddOptionAsync(Option option)
        {
            if (option.BasePrice <= 0)
                throw new Exception("Base price must be greater than zero.");

            await _optionRepository.AddAsync(option);
        }

        public async Task UpdateOptionAsync(Option option)
        {
            if (option.BasePrice <= 0)
                throw new Exception("Base price must be greater than zero.");

            await _optionRepository.UpdateAsync(option);
        }

        public async Task SetOptionStatusAsync(int optionId, bool isActive)
        {
            await _optionRepository.SetStatusAsync(optionId, isActive);
        }

    }
}
