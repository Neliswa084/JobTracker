using JobTracker.BusinessLogic.Interfaces;
using JobTracker.BusinessLogic.Services;
using JobTracker.DataLogic.Data;
using JobTracker.DataLogic.Models;
using JobTracker.Repository.Implementations;
using JobTracker.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// 1. Register the Database (Entity Framework Core + SQL Server)
// -----------------------------------------------------------------------
// We read the connection string from appsettings.json and tell EF Core to use SQL Server.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------------------------------------------------------
// 2. Register ASP.NET Core Identity (user accounts and authentication)
// -----------------------------------------------------------------------
// This sets up Identity to use our custom ApplicationUser class and store
// everything in our AppDbContext (SQL Server database).
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password rules - keeping them simple for a portfolio project
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>() // Store user data in our database
.AddDefaultTokenProviders();              // Needed for things like password reset tokens

// -----------------------------------------------------------------------
// 3. Register our own services (Dependency Injection)
// -----------------------------------------------------------------------
// This is how ASP.NET Core knows: "When someone asks for IJobApplicationRepository,
// give them a JobApplicationRepository". This is called Dependency Injection.
// AddScoped means one instance is created per web request.
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();

// -----------------------------------------------------------------------
// 4. Register MVC (Controllers + Views)
// -----------------------------------------------------------------------
builder.Services.AddControllersWithViews();

// -----------------------------------------------------------------------
// Build the app and configure the HTTP pipeline
// -----------------------------------------------------------------------
var app = builder.Build();

// In production, show a friendly error page instead of the raw exception
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve CSS, JS, images from wwwroot

app.UseRouting();

// Authentication must come BEFORE Authorization
app.UseAuthentication(); // Checks "who are you?" (reads the login cookie)
app.UseAuthorization();  // Checks "are you allowed to do this?"

// Default route: go to HomeController -> Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
