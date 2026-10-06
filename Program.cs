using Microsoft.EntityFrameworkCore;
using Tour_Module.Data;
using Tour_Module.Repository.Implementations;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Implementations;
using Tour_Module.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// DATABASE
// ============================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================================
// REPOSITORIES
// ============================================================

builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<ITourRepository>(
    _ => new TourRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<ILocationImageRepository, LocationImageRepository>();

builder.Services.AddScoped<IAuthRepository>(
    _ => new AuthRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IOptionRepository>(
    _ => new OptionRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<ITourAddOnRepository>(
    _ => new TourAddOnRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IUserTourRepository>(
    _ => new UserTourRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<ITourDraftRepository>(
    _ => new TourDraftRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!));

// ============================================================
// SERVICES
// ============================================================

builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<ITourService, TourService>();

builder.Services.AddScoped<ILocationImageService, LocationImageService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IOptionService, OptionService>();

builder.Services.AddScoped<ITourAddOnService, TourAddOnService>();

builder.Services.AddScoped<IUserTourService, UserTourService>();

builder.Services.AddScoped<IUserTourIndexService, UserTourIndexService>();

builder.Services.AddScoped<ITourDraftService, TourDraftService>();

// ============================================================
// MVC
// ============================================================

builder.Services.AddControllersWithViews();

// ============================================================
// AUTHENTICATION
// ============================================================

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.Cookie.Name = "TourApp.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy =
            CookieSecurePolicy.SameAsRequest;

        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ============================================================
// HTTP REQUEST PIPELINE
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// ============================================================
// AREA ROUTE
// ============================================================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// ============================================================
// DEFAULT ROUTE
// ============================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserTour}/{action=Index}/{id?}");

app.Run();