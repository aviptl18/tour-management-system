using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tour_Module.Models.Entities;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Location;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // List All Locations

        public async Task<IActionResult> Index()
        {
            var locations = await _locationService.GetAllAsync();
            return View(locations);
        }

        // Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateLocationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _locationService.CreateAsync(model.LocationName);

                TempData["Success"] = "Location created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _locationService.GetByIdAsync(id);

            if (location == null)
                return NotFound();

            var model = new EditLocationViewModel
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(EditLocationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _locationService.UpdateAsync(
                    model.LocationId,
                    model.LocationName
                );

                TempData["Success"] = "Location updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // Deactivate

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _locationService.DeactivateAsync(id);
                TempData["Success"] = "Location deactivated.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // Activate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _locationService.ActivateAsync(id);
                TempData["Success"] = "Location activated.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // Search Location (tour creation step 2)

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var locations = await _locationService.SearchAsync(term);

            var result = locations.Select(l => new
            {
                l.LocationId,
                l.LocationName
            });

            return Json(result);
        }
    }
}