using Microsoft.AspNetCore.Mvc;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Controllers
{
    public class ToursController : Controller
    {
        private readonly ITourService _adminTourService;     // For Index (temporary)
        private readonly IUserTourService _userTourService;  // For Details

        public ToursController(
            ITourService adminTourService,
            IUserTourService userTourService)
        {
            _adminTourService = adminTourService;
            _userTourService = userTourService;
        }

        // --------------------------------------------------
        // USER: TOUR LIST (TEMPORARY USING ADMIN READ)
        // --------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var tours = await _adminTourService.GetAllToursAsync();
            return View(tours);
        }

        // --------------------------------------------------
        // USER: TOUR DETAILS (USER SAFE)
        // --------------------------------------------------
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var tour = await _userTourService.GetUserTourDetailsAsync(id);

            if (tour == null)
                return NotFound();

            return View(tour);
        }
    }
}
