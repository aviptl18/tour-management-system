using Microsoft.AspNetCore.Mvc;
using Tour_Module.Models.Entities;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Options;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OptionsController : Controller
    {
        private readonly IOptionService _optionService;

        public OptionsController(IOptionService optionService)
        {
            _optionService = optionService;
        }

        public async Task<IActionResult> Index()
        {
            var options = await _optionService.GetAllOptionsAsync();

            var viewModel = options.Select(o => new OptionListViewModel
            {
                OptionId = o.OptionId,
                OptionName = o.OptionName,
                BasePrice = o.BasePrice,
                Currency = o.Currency,
                Category = o.Category,
                IsActive = o.IsActive
            }).ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new OptionCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OptionCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var option = new Option
            {
                OptionName = model.OptionName,
                BasePrice = model.BasePrice,
                Currency = model.Currency,
                Category = model.Category
            };

            await _optionService.CreateOptionAsync(option);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var option = await _optionService.GetOptionByIdAsync(id);

            if (option == null)
                return NotFound();

            var viewModel = new OptionEditViewModel
            {
                OptionId = option.OptionId,
                OptionName = option.OptionName,
                BasePrice = option.BasePrice,
                Currency = option.Currency,
                Category = option.Category,
                IsActive = option.IsActive
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OptionEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var option = new Option
            {
                OptionId = model.OptionId,
                OptionName = model.OptionName,
                BasePrice = model.BasePrice,
                Currency = model.Currency,
                Category = model.Category,
                IsActive = model.IsActive
            };

            await _optionService.UpdateOptionAsync(option);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _optionService.DeactivateOptionAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            await _optionService.ActivateOptionAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}