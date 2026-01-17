using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsBox.Data;
using NewsBox.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15);
}).AddDefaultTokenProviders().AddEntityFrameworkStores<NewsDbContext>();
builder.Services.AddDbContext<NewsDbContext>(options=> {
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=News;Trusted_Connection=True;TrustServerCertificate=True;");
});
var app = builder.Build();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
