using Microsoft.EntityFrameworkCore;
using MyShop.DAL;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ItemDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ItemDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ItemDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ItemDbContextConnection"]);
});

//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ItemDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
     // Password settings
     options.Password.RequireDigit = true;
     options.Password.RequiredLength = 8;
     options.Password.RequireNonAlphanumeric = true;
     options.Password.RequireUppercase = true;
     options.Password.RequireLowercase = true;
     options.Password.RequiredUniqueChars = 6;

     // Lockout settings
     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
     options.Lockout.MaxFailedAccessAttempts = 5;
     options.Lockout.AllowedForNewUsers = true;

     // User settings
     options.User.RequireUniqueEmail = true;

     // Sign-in settings
     options.SignIn.RequireConfirmedAccount = false; // Set to true if you want email confirmation
 })
 .AddEntityFrameworkStores<ItemDbContext>()
 .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
 {
     options.LoginPath = "/Identity/Account/Login"; // Ensure this path is valid
 });

builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddRazorPages(); // order of adding services does not matter
builder.Services.AddSession(); // Middleware that enables session state

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information() // levels: Trace< Information < Warning < Erorr < Fatal
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Check if the app is running in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Maps the deafult controller route for the application
    DBInit.Seed(app); // Seed the database with items
}

app.UseStaticFiles(); // Middleware that serves static files (images, css, etc.)

app.UseAuthentication(); // Middleware that authenticates the user

app.UseAuthorization(); // Middleware that authorizes the user

app.MapDefaultControllerRoute(); // Handles the routing of incoming requests

app.MapRazorPages(); // Handles the routing of incoming requests

app.Run(); // Middleware that runs the application