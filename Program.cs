var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // Add MVC services (components registered with ASP.NET) to the container

var app = builder.Build();

// Check if the app is running in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Maps the deafult controller route for the application
}

app.UseStaticFiles(); // Middleware that serves static files (images, css, etc.)

app.MapDefaultControllerRoute(); // Handles the routing of incoming requests

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Middleware that runs the application