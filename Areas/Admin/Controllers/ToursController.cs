using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Tour_Module.DTOs.Tours;
using Tour_Module.DTOs.Tours.View;
using Tour_Module.Models.Entities;
using Tour_Module.Services.Interfaces;
using System.Security.Claims;

namespace Tour_Module.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        // Purpose:
        //   - Shows all tours in admin panel
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
                throw new UnauthorizedAccessException("User ID claim not found.");

            return int.Parse(userIdClaim.Value);
        }


        // Add Create Entry Point

        public async Task<IActionResult> Create()
        {
            int adminId = GetCurrentAdminId();
            int draftId = await _tourDraftService.StartDraftAsync(adminId);

            return RedirectToAction(nameof(CreateStep1), new { draftId });
        }

        // ============================================================
        // STEP 1 — BASIC INFO
        // ============================================================
        public async Task<IActionResult> CreateStep1(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);

            if (draft == null)
                return RedirectToAction(nameof(Index));

            var viewModel = new CreateTourStep1ViewDto
            {
                DraftId = draft.DraftId,
                TourTitle = draft.TourTitle,
                Overview = draft.Overview,
                OriginalTourId = draft.OriginalTourId,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStep1(CreateTourStep1Dto input)
        {
            int adminId = GetCurrentAdminId();

            if (!ModelState.IsValid)
            {
                return View(new CreateTourStep1ViewDto
                {
                    DraftId = input.DraftId,
                    TourTitle = input.TourTitle,
                    Overview = input.Overview
                });
            }

            await _tourDraftService.SaveStep1Async(
                input.DraftId,
                adminId,
                input.TourTitle,
                input.Overview
            );

            return RedirectToAction(nameof(CreateStep2), new {draftId = input.DraftId});
        }

        // ============================================================
        // STEP 2 — LOCATION + IMAGES
        // ============================================================
        public async Task<IActionResult> CreateStep2(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);

            if (draft == null || draft.DraftStep < 1)
                return RedirectToAction(nameof(CreateStep1), new { draftId });

            var draftImages = await _tourDraftService.GetDraftImagesAsync(draftId);

            var selectedImageIds = draftImages.Select(x => x.LocationImageId).ToList();
            var coverImageId = draftImages
                .Where(x => x.IsCover)
                .Select(x => (int?)x.LocationImageId)
                .FirstOrDefault();

            var locations = await _locationService.GetAvailableForTourAsync();

            return View(new CreateTourStep2ViewDto
            {
                DraftId = draftId,
                LocationId = draft.LocationId ?? 0,
                SelectedImageIds = selectedImageIds,
                CoverImageId = coverImageId,
                OriginalTourId = draft.OriginalTourId,
                Locations = locations.Select(l => new SelectListItem
                {
                    Value = l.LocationId.ToString(),
                    Text = l.LocationName
                }).ToList()
            });
        }


        // POST 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStep2(CreateTourStep2Dto input)
        {
            int adminId = GetCurrentAdminId();

            var selectedImageIds = Request.Form["SelectedImageIds"]
                .Select(v => int.TryParse(v, out var id) ? id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            var coverIds = Request.Form["CoverImageId"]
                .Select(v => int.TryParse(v, out var id) ? id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            int? coverImageId = coverIds.Count == 1 ? coverIds[0] : null;

            // VALIDATION

            if (input.LocationId <= 0)
                ModelState.AddModelError(nameof(input.LocationId), "Location is required.");

            if (!selectedImageIds.Any())
                ModelState.AddModelError("SelectedImageIds", "Select at least one image.");

            if (coverImageId == null)
                ModelState.AddModelError("CoverImageId", "Select cover image.");

            if (coverImageId != null && !selectedImageIds.Contains(coverImageId.Value))
                ModelState.AddModelError("CoverImageId", "Cover must be selected from chosen images.");

            if (!ModelState.IsValid)
            {
                var locations = await _locationService.GetAvailableForTourAsync();

                return View(new CreateTourStep2ViewDto
                {
                    DraftId = input.DraftId,
                    LocationId = input.LocationId,
                    SelectedImageIds = selectedImageIds,
                    CoverImageId = coverImageId,
                    Locations = locations.Select(l => new SelectListItem
                    {
                        Value = l.LocationId.ToString(),
                        Text = l.LocationName
                    }).ToList()
                });
            }

            await _tourDraftService.SaveStep2Async(
                input.DraftId,
                adminId,
                input.LocationId,
                selectedImageIds,
                coverImageId.Value
            );

            return RedirectToAction(nameof(CreateStep3), new {draftId = input.DraftId});
        }


        // ============================================================
        // STEP 3 — PRICING
        // ============================================================
        public async Task<IActionResult> CreateStep3(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);

            if (draft == null || draft.DraftStep < 2)
                return RedirectToAction(nameof(CreateStep2), new { draftId });

            // 🔹 ensure base component exists
            var components = await _tourDraftService.GetStep3ComponentsAsync(draftId);

            // 🔹 load active options
            var options = await _optionService.GetActiveOptionsAsync();

            // 🔹 remove already added options
            var selectedOptionIds = components
                .Where(c => c.OptionId.HasValue)
                .Select(c => c.OptionId.Value)
                .ToList();

            var availableOptions = options
                .Where(o => !selectedOptionIds.Contains(o.OptionId))
                .ToList();

            var viewModel = new CreateTourStep3ViewDto
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



        [HttpGet]
        public async Task<IActionResult> GetAvailableOptions(int draftId)
        {
            var components = await _tourDraftService.GetStep3ComponentsAsync(draftId);
            var selectedIds = components
                .Where(c => c.OptionId.HasValue)
                .Select(c => c.OptionId.Value)
                .ToList();

            var options = await _optionService.GetActiveOptionsAsync();

            var available = options
                .Where(o => !selectedIds.Contains(o.OptionId))
                .Select(o => new {
                    optionId = o.OptionId,
                    optionName = o.OptionName,
                    basePrice = o.BasePrice
                });

            return Json(available);
        }


        // AJAX - Add Option

        [HttpPost]
        public async Task<IActionResult> AddPricingOption(int draftId, int optionId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);

            if (draft == null)
                return BadRequest("Invalid Draft");

            await _tourDraftService.AddOptionAsync(draftId, optionId);

            var components = await _tourDraftService.GetStep3ComponentsAsync(draftId);

            return PartialView("_PricingComponentsPartial", components);
        }

        // AJAX - Remove Option

        [HttpPost]
        public async Task<IActionResult> RemovePricingOption(int componentId, int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);

            if (draft == null)
                return BadRequest("Invalid Draft");

            await _tourDraftService.RemoveOptionAsync(componentId);

            var components = await _tourDraftService.GetStep3ComponentsAsync(draftId);

            return PartialView("_PricingComponentsPartial", components);

        }

        // AJAX - Update BasePrice

        [HttpPost]
        public async Task<IActionResult> UpdateBasePrice(int componentId, decimal price, int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);
            if (draft == null)
                return BadRequest("Invalid draft.");

            try
            {
                await _tourDraftService.UpdateComponentPriceAsync(componentId, price);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            var components = await _tourDraftService.GetStep3ComponentsAsync(draftId);
            return PartialView("_PricingComponentsPartial", components);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteStep3(CreateTourStep3Dto dto)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(dto.DraftId, adminId);

            if (draft == null)
                return RedirectToAction(nameof(Index));

            // 1️⃣ Validate duration
            if (!ModelState.IsValid)
                return await ReloadStep3(dto.DraftId, dto.Duration);

            // 2️⃣ Validate pricing
            var pricingValid = await _tourDraftService.ValidatePricingAsync(dto.DraftId);

            if (!pricingValid)
            {
                ModelState.AddModelError("", "Base price must be greater than zero.");
                return await ReloadStep3(dto.DraftId, dto.Duration);
            }

            // 3️⃣ Save duration
            await _tourDraftService.SaveStep3Async(dto.DraftId, dto.Duration);

            // 4️⃣ Mark step complete
            await _tourDraftService.CompleteStep3Async(dto.DraftId);

            TempData["Success"] = "Step 3 saved successfully.";

            // Stay on Step 3
            return RedirectToAction(nameof(CreateStep3), new { draftId = dto.DraftId });
        }

        // Add Private helper

        private async Task<IActionResult> ReloadStep3(int draftId, string duration)
        {
            var components = await _tourDraftService.GetStep3ComponentsAsync(draftId);
            var options = await _optionService.GetActiveOptionsAsync();

            var selectedOptionIds = components
                .Where(c => c.OptionId.HasValue)
                .Select(c => c.OptionId.Value)
                .ToList();

            var availableOptions = options
                .Where(o => !selectedOptionIds.Contains(o.OptionId))
                .ToList();

            var draft = await _tourDraftService.GetDraftAsync(draftId, GetCurrentAdminId());

            var vm = new CreateTourStep3ViewDto
            {
                DraftId = draftId,
                Duration = duration,
                Components = components,
                AvailableOptions = availableOptions,
                Status = draft.Status,
                OriginalTourId = draft.OriginalTourId
            };

            return View("CreateStep3", vm);
        }

        // ============================================================
        // ADMIN: TOUR DETAILS
        // Purpose:
        //   - Shows read-only tour details page
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var tour = await _tourService.GetAdminTourDetailAsync(id);

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
            var images = await _locationImageService.GetByLocationIdAsync(id);
            return Json(images.Select(i => new { i.ImageId, i.ImageUrl }));
        }

        // ============================================================
        // IMAGE UPLOAD
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> UploadLocationImages(
            int locationId,
            List<IFormFile> files)
        {
            if (locationId <= 0 || files == null || !files.Any())
                return BadRequest();

            foreach (var file in files)
            {
                var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var folder = Path.Combine(
                    _env.WebRootPath,
                    "Images",
                    "Locations",
                    locationId.ToString());

                Directory.CreateDirectory(folder);

                var path = Path.Combine(folder, name);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                await _locationImageService.AddAsync(new LocationImage
                {
                    LocationId = locationId,
                    ImageUrl = $"/Images/Locations/{locationId}/{name}"
                });
            }

            return Json(
                await _locationImageService.GetByLocationIdAsync(locationId));
        }

        // Save Draft

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var success = await _tourDraftService.CompleteDraftAsync(draftId, adminId);

            return Json(new { success });
        }

        //Cancel Draft

        [HttpPost]
        public async Task<IActionResult> CancelDraft(int draftId)
        {
            int adminId = GetCurrentAdminId();

            await _tourDraftService.CancelDraftAsync(draftId, adminId);

            return RedirectToAction(nameof(Index));
        }

        // Publish Tour

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishTour(int draftId)
        {
            try
            {
                // TODO: replace with logged-in admin id
                int adminId = GetCurrentAdminId();

                int newTourId = await _tourService.PublishTourAsync(draftId, adminId);

                TempData["Success"] = "Tour published successfully!";

                return RedirectToAction("Index"); // Tour list
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateStep3", new { draftId });
            }
        }

        public async Task<IActionResult> Edit (int tourId)
        {
            int adminId = GetCurrentAdminId();

            int draftId = await _tourService.GetOrCreateEditDraftAsync(tourId, adminId);

            return RedirectToAction("CreateStep1", new { draftId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTour(int draftId)
        {
            int adminId = GetCurrentAdminId();

            var draft = await _tourDraftService.GetDraftAsync(draftId, adminId);

            if (draft == null)
                return Unauthorized();

            await _tourService.UpdateTourFromDraftAsync(draftId, adminId);

            TempData["Success"] = "Tour updated successfully.";

            return RedirectToAction("Index");
        }
    }
}
