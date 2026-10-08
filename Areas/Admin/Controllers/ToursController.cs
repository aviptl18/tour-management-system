using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Tour_Module.Constants;
using Tour_Module.DTOs.Tours;
using Tour_Module.Models.Entities;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Admin.Tours;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class ToursController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ILocationService _locationService;
        private readonly ILocationImageService _locationImageService;
        private readonly IWebHostEnvironment _env;
        private readonly ITourDraftService _tourDraftService;
        private readonly IOptionService _optionService;

        public ToursController(
            ITourService tourService,
            ILocationService locationService,
            ILocationImageService locationImageService,
            IWebHostEnvironment env,
            ITourDraftService tourDraftService,
            IOptionService optionService)
        {
            _tourService = tourService;
            _locationService = locationService;
            _locationImageService = locationImageService;
            _env = env;
            _tourDraftService = tourDraftService;
            _optionService = optionService;
        }

        // ============================================================
        // ADMIN: LIST ALL TOURS
        // ============================================================

        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllToursAsync();

            return View(tours);
        }

        private int GetCurrentAdminId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException(
                    "User ID claim not found.");

            return int.Parse(userIdClaim.Value);
        }

        // ============================================================
        // CREATE TOUR - ENTRY POINT
        // ============================================================

        public async Task<IActionResult> Create()
        {
            int adminId = GetCurrentAdminId();

            int draftId =
                await _tourDraftService.StartDraftAsync(adminId);

            return RedirectToAction(
                nameof(CreateStep1),
                new { draftId });
        }

        // ============================================================
        // STEP 1 — BASIC INFO
        // ============================================================

        public async Task<IActionResult> CreateStep1(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null)
                return RedirectToAction(nameof(Index));

            var viewModel = new CreateStep1ViewModel
            {
                DraftId = draft.DraftId,
                TourTitle = draft.TourTitle,
                Overview = draft.Overview,
                OriginalTourId = draft.OriginalTourId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStep1(
            CreateStep1ViewModel input)
        {
            int adminId = GetCurrentAdminId();

            if (!ModelState.IsValid)
            {
                return View(new CreateStep1ViewModel
                {
                    DraftId = input.DraftId,
                    TourTitle = input.TourTitle,
                    Overview = input.Overview,
                    OriginalTourId = input.OriginalTourId
                });
            }

            await _tourDraftService.SaveStep1Async(
                input.DraftId,
                adminId,
                input.TourTitle,
                input.Overview);

            return RedirectToAction(
                nameof(CreateStep2),
                new { draftId = input.DraftId });
        }

        // ============================================================
        // STEP 2 — LOCATION + IMAGES
        // ============================================================

        public async Task<IActionResult> CreateStep2(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null || draft.DraftStep < 1)
            {
                return RedirectToAction(
                    nameof(CreateStep1),
                    new { draftId });
            }

            var draftImages =
                await _tourDraftService.GetDraftImagesAsync(
                    draftId,
                    adminId);

            var selectedImageIds =
                draftImages
                    .Select(x => x.LocationImageId)
                    .ToList();

            var coverImageId =
                draftImages
                    .Where(x => x.IsCover)
                    .Select(x => (int?)x.LocationImageId)
                    .FirstOrDefault();

            var locations =
                await _locationService.GetAvailableForTourAsync();

            return View(new CreateStep2ViewModel
            {
                DraftId = draftId,
                LocationId = draft.LocationId ?? 0,
                SelectedImageIds = selectedImageIds,
                CoverImageId = coverImageId,
                OriginalTourId = draft.OriginalTourId,

                Locations = locations
                    .Select(l => new SelectListItem
                    {
                        Value = l.LocationId.ToString(),
                        Text = l.LocationName
                    })
                    .ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStep2(
            CreateTourStep2Dto input)
        {
            int adminId = GetCurrentAdminId();

            var selectedImageIds =
                Request.Form["SelectedImageIds"]
                    .Select(v =>
                        int.TryParse(v, out var id)
                            ? id
                            : 0)
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();

            var coverIds =
                Request.Form["CoverImageId"]
                    .Select(v =>
                        int.TryParse(v, out var id)
                            ? id
                            : 0)
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();

            int? coverImageId =
                coverIds.Count == 1
                    ? coverIds[0]
                    : null;

            // ========================================================
            // VALIDATION
            // ========================================================

            if (input.LocationId <= 0)
            {
                ModelState.AddModelError(
                    nameof(input.LocationId),
                    "Location is required.");
            }

            if (!selectedImageIds.Any())
            {
                ModelState.AddModelError(
                    "SelectedImageIds",
                    "Select at least one image.");
            }

            if (coverImageId == null)
            {
                ModelState.AddModelError(
                    "CoverImageId",
                    "Select cover image.");
            }

            if (coverImageId != null &&
                !selectedImageIds.Contains(coverImageId.Value))
            {
                ModelState.AddModelError(
                    "CoverImageId",
                    "Cover must be selected from chosen images.");
            }

            // ========================================================
            // RETURN VIEW IF INVALID
            // ========================================================

            if (!ModelState.IsValid)
            {
                var locations =
                    await _locationService.GetAvailableForTourAsync();

                return View(new CreateStep2ViewModel
                {
                    DraftId = input.DraftId,
                    LocationId = input.LocationId,
                    SelectedImageIds = selectedImageIds,
                    CoverImageId = coverImageId,

                    Locations = locations
                        .Select(l => new SelectListItem
                        {
                            Value = l.LocationId.ToString(),
                            Text = l.LocationName
                        })
                        .ToList()
                });
            }

            // ========================================================
            // SAVE STEP 2
            // ========================================================

            await _tourDraftService.SaveStep2Async(
                input.DraftId,
                adminId,
                input.LocationId,
                selectedImageIds,
                coverImageId.Value);

            return RedirectToAction(
                nameof(CreateStep3),
                new { draftId = input.DraftId });
        }

        // ============================================================
        // STEP 3 — PRICING
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> CreateStep3(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null || draft.DraftStep < 2)
            {
                return RedirectToAction(
                    nameof(CreateStep2),
                    new { draftId });
            }

            var components =
                await _tourDraftService.GetStep3ComponentsAsync(
                    draftId,
                    adminId);

            var options =
                await _optionService.GetActiveOptionsAsync();

            var selectedOptionIds =
                components
                    .Where(c => c.OptionId.HasValue)
                    .Select(c => c.OptionId.Value)
                    .ToList();

            var availableOptions =
                options
                    .Where(o =>
                        !selectedOptionIds.Contains(o.OptionId))
                    .ToList();

            var viewModel = new CreateStep3ViewModel
            {
                DraftId = draftId,
                Duration = draft.Duration,
                Components = components,
                AvailableOptions = availableOptions,
                Status = draft.Status,
                OriginalTourId = draft.OriginalTourId
            };

            return View(viewModel);
        }

        // ============================================================
        // GET AVAILABLE OPTIONS
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> GetAvailableOptions(
            int draftId)
        {
            int adminId = GetCurrentAdminId();

            var components =
                await _tourDraftService.GetStep3ComponentsAsync(
                    draftId,
                    adminId);

            var selectedIds =
                components
                    .Where(c => c.OptionId.HasValue)
                    .Select(c => c.OptionId.Value)
                    .ToList();

            var options =
                await _optionService.GetActiveOptionsAsync();

            var available =
                options
                    .Where(o =>
                        !selectedIds.Contains(o.OptionId))
                    .Select(o => new
                    {
                        optionId = o.OptionId,
                        optionName = o.OptionName,
                        basePrice = o.BasePrice
                    });

            return Json(available);
        }

        // ============================================================
        // AJAX - ADD OPTION
        // ============================================================

        [HttpPost]
        public async Task<IActionResult> AddPricingOption(
            int draftId,
            int optionId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null)
                return BadRequest("Invalid Draft");

            await _tourDraftService.AddOptionAsync(
                draftId,
                adminId,
                optionId);

            var components =
                await _tourDraftService.GetStep3ComponentsAsync(
                    draftId,
                    adminId);

            return PartialView(
                "_PricingComponentsPartial",
                components);
        }

        // ============================================================
        // AJAX - REMOVE OPTION
        // ============================================================

        [HttpPost]
        public async Task<IActionResult> RemovePricingOption(
            int componentId,
            int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null)
                return BadRequest("Invalid Draft");

            await _tourDraftService.RemoveOptionAsync(
                componentId,
                adminId);

            var components =
                await _tourDraftService.GetStep3ComponentsAsync(
                    draftId,
                    adminId);

            return PartialView(
                "_PricingComponentsPartial",
                components);
        }

        // ============================================================
        // AJAX - UPDATE BASE PRICE
        // ============================================================

        [HttpPost]
        public async Task<IActionResult> UpdateBasePrice(
            int componentId,
            decimal price,
            int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null)
                return BadRequest("Invalid draft.");

            try
            {
                await _tourDraftService.UpdateBasePriceAsync(
                    draftId,
                    adminId,
                    componentId,
                    price);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            var components =
                await _tourDraftService.GetStep3ComponentsAsync(
                    draftId,
                    adminId);

            return PartialView(
                "_PricingComponentsPartial",
                components);
        }

        // ============================================================
        // COMPLETE STEP 3
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteStep3(
            CreateTourStep3Dto dto)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    dto.DraftId,
                    adminId);

            if (draft == null)
                return RedirectToAction(nameof(Index));

            // ========================================================
            // VALIDATE DURATION
            // ========================================================

            if (!ModelState.IsValid)
            {
                return await ReloadStep3(
                    dto.DraftId,
                    dto.Duration);
            }

            // ========================================================
            // VALIDATE PRICING
            // ========================================================

            var pricingValid =
                await _tourDraftService.ValidatePricingAsync(
                    dto.DraftId,
                    adminId);

            if (!pricingValid)
            {
                ModelState.AddModelError(
                    "",
                    "Base price must be greater than zero.");

                return await ReloadStep3(
                    dto.DraftId,
                    dto.Duration);
            }

            // ========================================================
            // SAVE DURATION
            // ========================================================

            await _tourDraftService.SaveStep3Async(
                dto.DraftId,
                adminId,
                dto.Duration!);

            // ========================================================
            // MARK STEP 3 COMPLETE
            // ========================================================

            await _tourDraftService.CompleteStep3Async(
                dto.DraftId,
                adminId);

            TempData["Success"] =
                "Step 3 saved successfully.";

            return RedirectToAction(
                nameof(CreateStep3),
                new { draftId = dto.DraftId });
        }

        // ============================================================
        // RELOAD STEP 3
        // ============================================================

        private async Task<IActionResult> ReloadStep3(
            int draftId,
            string? duration)
        {
            int adminId = GetCurrentAdminId();

            var components =
                await _tourDraftService.GetStep3ComponentsAsync(
                    draftId,
                    adminId);

            var options =
                await _optionService.GetActiveOptionsAsync();

            var selectedOptionIds =
                components
                    .Where(c => c.OptionId.HasValue)
                    .Select(c => c.OptionId.Value)
                    .ToList();

            var availableOptions =
                options
                    .Where(o =>
                        !selectedOptionIds.Contains(o.OptionId))
                    .ToList();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null)
                return RedirectToAction(nameof(Index));

            var viewModel = new CreateStep3ViewModel
            {
                DraftId = draftId,
                Duration = duration,
                Components = components,
                AvailableOptions = availableOptions,
                Status = draft.Status,
                OriginalTourId = draft.OriginalTourId
            };

            return View(
                "CreateStep3",
                viewModel);
        }

        // ============================================================
        // ADMIN: TOUR DETAILS
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var tour =
                await _tourService.GetTourDetailsAsync(id);

            if (tour == null)
                return NotFound();

            return View(tour);
        }

        // ============================================================
        // AJAX — LOCATION IMAGES
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> GetImagesByLocation(int id)
        {
            var images =
                await _locationImageService.GetByLocationIdAsync(id);

            return Json(
                images.Select(i => new
                {
                    i.ImageId,
                    i.ImageUrl
                }));
        }

        // ============================================================
        // IMAGE UPLOAD
        // ============================================================

        [HttpPost]
        public async Task<IActionResult> UploadLocationImages(
            int locationId,
            List<IFormFile> files)
        {
            if (locationId <= 0 ||
                files == null ||
                !files.Any())
            {
                return BadRequest();
            }

            foreach (var file in files)
            {
                var name =
                    $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                var folder =
                    Path.Combine(
                        _env.WebRootPath,
                        "Images",
                        "Locations",
                        locationId.ToString());

                Directory.CreateDirectory(folder);

                var path =
                    Path.Combine(folder, name);

                using var stream =
                    new FileStream(
                        path,
                        FileMode.Create);

                await file.CopyToAsync(stream);

                await _locationImageService.AddAsync(
                    new LocationImage
                    {
                        LocationId = locationId,
                        ImageUrl =
                            $"/Images/Locations/{locationId}/{name}"
                    });
            }

            return Json(
                await _locationImageService
                    .GetByLocationIdAsync(locationId));
        }

        // ============================================================
        // SAVE DRAFT
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(
            int draftId)
        {
            int adminId = GetCurrentAdminId();

            var success =
                await _tourDraftService.CompleteDraftAsync(
                    draftId,
                    adminId);

            return Json(new { success });
        }

        // ============================================================
        // CANCEL DRAFT
        // ============================================================

        [HttpPost]
        public async Task<IActionResult> CancelDraft(
            int draftId)
        {
            int adminId = GetCurrentAdminId();

            await _tourDraftService.CancelDraftAsync(
                draftId,
                adminId);

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // PUBLISH TOUR
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishTour(
            int draftId)
        {
            try
            {
                int adminId = GetCurrentAdminId();

                int newTourId =
                    await _tourService.PublishTourAsync(
                        draftId,
                        adminId);

                TempData["Success"] =
                    "Tour published successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    nameof(CreateStep3),
                    new { draftId });
            }
        }

        // ============================================================
        // EDIT TOUR
        // ============================================================

        public async Task<IActionResult> Edit(
            int tourId)
        {
            int adminId = GetCurrentAdminId();

            int draftId =
                await _tourService.GetOrCreateEditDraftAsync(
                    tourId,
                    adminId);

            return RedirectToAction(
                nameof(CreateStep1),
                new { draftId });
        }

        // ============================================================
        // UPDATE TOUR FROM DRAFT
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTour(
            int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft =
                await _tourDraftService.GetDraftAsync(
                    draftId,
                    adminId);

            if (draft == null)
                return Unauthorized();

            await _tourService.UpdateTourFromDraftAsync(
                draftId,
                adminId);

            TempData["Success"] =
                "Tour updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // DELETE TOUR
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
                return NotFound();

            await _tourService.DeleteTourAsync(id);

            return RedirectToAction(nameof(Index));
        }

    }
}