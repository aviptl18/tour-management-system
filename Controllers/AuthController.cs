using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Tour_Module.DTOs.Auth;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ---------------- LOGIN ----------------

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var principal = await _authService.AuthenticateAsync(dto);

            if (principal == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(dto);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            // Role-based redirect
            if (principal.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Tours", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Tours");
        }

        // Register Get

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Register Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var success = await _authService.RegisterAsync(dto);

            if (!success)
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(dto);
            }

            return RedirectToAction("Login", "Auth");
        }


        // ---------------- LOGOUT ----------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login", "Auth");
        }

        // ---------------- ACCESS DENIED ----------------

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
