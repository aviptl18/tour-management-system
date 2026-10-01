using Microsoft.AspNetCore.Mvc;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourAddOnsController : Controller
    {
        private readonly ITourAllowedOptionService _tourAllowedOptionService;

        public TourAddOnsController(
            ITourAllowedOptionService tourAllowedOptionService)
        {
            _tourAllowedOptionService = tourAllowedOptionService;
        }

        // =====================================================
        // GET: Admin/TourAddOns/Configure
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> Configure(int tourId)
        {
            if (tourId <= 0)
                return BadRequest();

            var vm = await _tourAllowedOptionService
                .GetConfigurationDataAsync(tourId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // =====================================================
        // GET: Available Optional Add-Ons (AJAX)
        // =====================================================
        [HttpGet]
        public async Task<IActionResult>
            GetAvailableOptionalOptions(int tourId)
        {
            if (tourId <= 0)
                return BadRequest();

            var options = await _tourAllowedOptionService
                .GetAvailableOptionalOptionsForTourAsync(tourId);

            return Json(options);
        }

        // =====================================================
        // POST: Add Optional Add-On (AJAX)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            AddOptionalAddOn([FromBody] AddRemoveAddOnRequest request)
        {
            if (request == null || request.TourId <= 0)
                return BadRequest();

            await _tourAllowedOptionService
                .AllowOptionForTourAsync(
                    request.TourId,
                    request.OptionId,
                    false); // always optional here

            return Ok();
        }

        // =====================================================
        // POST: Remove Optional Add-On (AJAX)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            RemoveOptionalAddOn([FromBody] AddRemoveAddOnRequest request)
        {
            if (request == null || request.TourId <= 0)
                return BadRequest();

            await _tourAllowedOptionService
                .RemoveOptionFromTourAsync(
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