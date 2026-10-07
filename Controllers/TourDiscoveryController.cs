using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tour_Module.DTOs.TourDiscovery;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.User.TourDiscovery;

namespace Tour_Module.Controllers
{
    public class TourDiscoveryController : Controller
    {
        private readonly ITourDiscoveryService _tourDiscoveryService;
        private readonly ILocationService _locationService;

        public TourDiscoveryController(
             ITourDiscoveryService tourDiscoveryService,
             ILocationService locationService)
        {
            _tourDiscoveryService = tourDiscoveryService;
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            TourDiscoveryFilterViewModel filter)
        {
            var dtoFilter = new TourDiscoveryFilterDto
            {
                SearchTerm = filter.SearchTerm,
                LocationName = filter.LocationName,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                SortBy = filter.SortBy,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            var result =
                await _tourDiscoveryService.SearchAsync(dtoFilter);

            var locations =
                await _locationService.GetAvailableForTourAsync();

            var totalPages =
                (int)Math.Ceiling(
                    result.TotalCount /
                    (double)filter.PageSize);

            var viewModel = new TourDiscoveryViewModel
            {
                Filter = filter,
                Tours = result.Tours,
                TotalCount = result.TotalCount,
                TotalPages = totalPages,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,

                LocationNames = locations
                    .Select(location => location.LocationName)
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var tour =
                await _tourDiscoveryService.GetDetailsAsync(id);

            if (tour == null)
                return NotFound();

            return View(tour);
        }
    }
}