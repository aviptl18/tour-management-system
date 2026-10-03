using Microsoft.AspNetCore.Mvc;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourAddOnsController : Controller
    {
        private readonly ITourAddOnService _tourAddOnService;

        public TourAddOnsController(
            ITourAddOnService tourAddOnService)
        {
            _tourAddOnService = tourAddOnService;
        }

        // =====================================================
        // GET: Admin/TourAddOns/Configure
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Configure(int tourId)
        {
            if (tourId <= 0)
                return BadRequest();

            var vm = await _tourAddOnService
                .GetConfigurationAsync(tourId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // =====================================================
        // GET: Available Add-Ons (AJAX)
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> GetAvailableAddOns(int tourId)
        {
            if (tourId <= 0)
                return BadRequest();

            var addOns = await _tourAddOnService
                .GetAvailableAddOnsAsync(tourId);

            return Json(addOns);
        }

        // =====================================================
        // POST: Add Optional Add-On (AJAX)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOptionalAddOn(
            [FromBody] AddRemoveAddOnRequest request)
        {
            if (request == null ||
                request.TourId <= 0 ||
                request.OptionId <= 0)
            {
                return BadRequest();
            }

            await _tourAddOnService
                .AddOptionalAddOnAsync(
                    request.TourId,
                    request.OptionId);

            return Ok();
        }

        // =====================================================
        // POST: Remove Optional Add-On (AJAX)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveOptionalAddOn(
            [FromBody] AddRemoveAddOnRequest request)
        {
            if (request == null ||
                request.TourId <= 0 ||
                request.OptionId <= 0)
            {
                return BadRequest();
            }

            await _tourAddOnService
                .RemoveOptionalAddOnAsync(
                    request.TourId,
                    request.OptionId);

            return Ok();
        }
    }

    // =========================================================
    // Request DTO for AJAX Add / Remove
    // =========================================================
    public class AddRemoveAddOnRequest
    {
        public int TourId { get; set; }

        public int OptionId { get; set; }
    }
}