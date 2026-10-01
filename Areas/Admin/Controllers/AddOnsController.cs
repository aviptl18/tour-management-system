using Microsoft.AspNetCore.Mvc;
using Tour_Module.ViewModels.AddOns;
using Tour_Module.Services.Interfaces;
using System.Net.Sockets;
using Tour_Module.Models.Entities;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AddOnsController : Controller
    {
        private readonly IOptionService _optionService;

        public AddOnsController(IOptionService optionService)
        {
            _optionService = optionService;
        }

        // Get : Admin/AddOns

        public async Task<IActionResult> Index()
        {
            var options = await _optionService.GetAllOptionsAsync();

            var viewModel = options.Select(o => new AddOnListViewModel
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

        // Get : Admin/AddOns/Create

        public IActionResult Create()
        {
            return View(new AddOnCreateEditViewModel());
        }

        // Post : Admin/AddOns/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddOnCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var option = new Option
            {
                OptionName = model.OptionName,
                BasePrice = model.BasePrice,
                Currency = model.Currency,
                Category = model.Category,
                IsActive = true
            };

            await _optionService.AddOptionAsync(option);
            return RedirectToAction(nameof(Index));
        }

        // Get : Admin/AddOns/Edit

        public async Task<IActionResult> Edit(int id)
        {
            var option = (await _optionService.GetAllOptionsAsync()).FirstOrDefault(o => o.OptionId == id );

            if (option == null)
                return NotFound();

            var viewModel = new AddOnCreateEditViewModel
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

        // POST : Admin/Addons/Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddOnCreateEditViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var option = new Option
            {
                OptionId = model.OptionId.Value,
                OptionName = model.OptionName,
                BasePrice = model.BasePrice,
                Currency = model.Currency,
                Category = model.Category,
                IsActive = model.IsActive
            };

            await _optionService.UpdateOptionAsync(option);
            return RedirectToAction(nameof(Index));
        }

        // POST : Admin/AddOns/SetOptionStatus

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _optionService.SetOptionStatusAsync(id,false);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            await _optionService.SetOptionStatusAsync(id, true);
            return RedirectToAction(nameof(Index));
        }
    }
}
