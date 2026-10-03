using Microsoft.EntityFrameworkCore;
using Tour_Module.Data;
using Tour_Module.Repository.Implementations;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Implementations;
using Tour_Module.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add DbContext

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories

builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<ITourRepository, TourRepository>();

builder.Services.AddScoped<ILocationImageRepository, LocationImageRepository>();

builder.Services.AddScoped<IAuthRepository>(
    _ => new AuthRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")!
    ));

builder.Services.AddScoped<IOptionRepository>(
    _ => new OptionRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITourAddOnRepository>(
    _ => new TourAddOnRepository(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserTourRepository, UserTourRepository>();

builder.Services.AddScoped<ITourDraftRepository, TourDraftRepository>();


// Services

builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<ITourService, TourService>();

builder.Services.AddScoped<ILocationImageService, LocationImageService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IOptionService, OptionService>();

builder.Services.AddScoped<ITourAddOnService, TourAddOnService>();

builder.Services.AddScoped<IUserTourService, UserTourService>();

builder.Services.AddScoped<IUserTourIndexService, UserTourIndexService>();

builder.Services.AddScoped<ITourDraftService, TourDraftService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.Cookie.Name = "TourApp.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();


//  AREA ROUTE (MUST COME FIRST)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tours}/{action=Index}/{id?}");

app.Run();

// NEED TO REMOVE SESSION MIDDLEWARE