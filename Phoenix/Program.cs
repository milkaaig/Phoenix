using Microsoft.EntityFrameworkCore;
using Phoenix.Models;
using Phoenix.Interfaces;
using Phoenix.Controllers;
using Phoenix.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Remove default logging providers and use only Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
);

//builder.Logging.ClearProviders(); // Removes all default logging providers

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IAddFunctions, AddFunctions>();    

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AddPost}/{action=GetPosts}/{id?}");

app.Run();
