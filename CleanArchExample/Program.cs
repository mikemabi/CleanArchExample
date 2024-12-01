using CleanArchExample.Application.Interfaces;
using CleanArchExample.Application.Services;
using CleanArchExample.Core.Entities;
using CleanArchExample.Core.Interfaces;
using CleanArchExample.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDBContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o=>o.SignIn.RequireConfirmedAccount = false).
    AddDefaultTokenProviders().AddEntityFrameworkStores<MyDBContext>();
builder.Services.Configure<IdentityOptions>(o => 
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase= true;
    o.Password.RequireUppercase= true;
    o.Password.RequireNonAlphanumeric = true;
    o.Password.RequiredLength= 8;
    o.Password.RequiredUniqueChars = 1;

    // Lockout Settings
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    o.Lockout.MaxFailedAccessAttempts = 5;
    o.Lockout.AllowedForNewUsers = true;

    //User Settings
    o.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    o.User.RequireUniqueEmail=false;

    //Singin Settings
    o.SignIn.RequireConfirmedEmail = false;


});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
