using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tour_Module.Constants;
using Tour_Module.DTOs.Authentication;
using Tour_Module.Results;
using Tour_Module.Services.Interfaces;
using Tour_Module.ViewModels.Authentication;
using IAuthenticationService = Tour_Module.Services.Interfaces.IAuthenticationService;

namespace Tour_Module.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel model = new()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LoginRequestDto request = new()
            {
                Email = model.Email,
                Password = model.Password
            };

            AuthenticationResult result =
                await _authenticationService
                    .AuthenticateAsync(request);

            if (!result.Succeeded)
            {
                if (result.LockedOut)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Your account is temporarily locked. Please try again later.");
                }
                else if (result.Inactive)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Your account is inactive.");
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Invalid email or password.");
                }

                return View(model);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                result.Principal!);

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl)
                && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            if (result.Principal!.IsInRole(RoleConstants.Admin))
            {
                return RedirectToAction(
                    "Index",
                    "Tours",
                    new { area = "Admin" });
            }

            return RedirectToAction(
                "Index",
                "TourDiscovery");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RegisterRequestDto request = new()
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password
            };

            AuthenticationResult result =
                await _authenticationService
                    .RegisterAsync(request);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registration failed.");

                return View(model);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                result.Principal!);

            if (result.Principal!.IsInRole(RoleConstants.Admin))
            {
                return RedirectToAction(
                    "Index",
                    "Tours",
                    new { area = "Admin" });
            }

            return RedirectToAction(
                "Index",
                "TourDiscovery");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            string? userId =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation(
                "User {UserId} logged out.",
                userId);

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(
                "Login",
                "Authentication");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}