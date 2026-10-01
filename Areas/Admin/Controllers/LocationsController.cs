using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tour_Module.Models.Entities;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
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
            var locations = await _locationService.GetAllLocationsForAdminAsync();
            return View(locations);
        }

        // Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string locationName)
        {
            try
            {
                await _locationService.CreateLocationAsync(locationName);
                TempData["Success"] = "Location created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Location model)
        {
            try
            {
                await _locationService.UpdateLocationAsync(
                    model.LocationId,
                    model.LocationName,
                    true
                );
                TempData["Success"] = "Location updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(model);
            }
        }

        // Deactivate

        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _locationService.DeactivateLocationAsync(id);
                TempData["Success"] = "Location deactivated.";
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // Seach Location ( tour creation step 2 ) 

        [HttpGet]
        public async Task<IActionResult> Search (string term)
        {
            var locations = await _locationService.SearchLocationsAsync(term);

            var result = locations.Select(l => new
            {
                l.LocationId,
                l.LocationName
            });

            return Json(result);
        }
    }
}
