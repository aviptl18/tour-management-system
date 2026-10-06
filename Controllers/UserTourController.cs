using Microsoft.AspNetCore.Mvc;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Controllers
{
    public class UserTourController : Controller
    {
        private readonly IUserTourIndexService _userTourIndexService;
        private readonly IUserTourService _userTourService;

        public UserTourController(
            IUserTourIndexService userTourIndexService,
            IUserTourService userTourService)
        {
            _userTourIndexService = userTourIndexService;
            _userTourService = userTourService;
        }

        // ============================================================
        // USER: TOUR LIST
        // ============================================================

        public async Task<IActionResult> Index()
        {
            var tours =
                await _userTourIndexService.GetActiveToursAsync();

            return View(tours);
        }

        // ============================================================
        // USER: TOUR DETAILS
        // ============================================================

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var tour =
                await _userTourService.GetUserTourDetailsAsync(id);

            if (tour == null)
                return NotFound();

            return View(tour);
        }
    }
}