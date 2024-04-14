using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using System.Data;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("ProjectDBContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'ProjectDBContextConnection' " +
    "not found.");

builder.Services.AddDbContext<ProjectDBContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
//options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ProjectDBContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddDefaultTokenProviders()
        .AddDefaultUI()
        .AddEntityFrameworkStores<ProjectDBContext>();
builder.Services.AddRazorPages();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<IboTruyenRepository, EFboTruyenRepository>();
builder.Services.AddScoped<ILoaiTruyenRepository, EFLoaiTruyenRepository>();
builder.Services.AddScoped<ITacGiaRepository, EFTacGiaRepository>();
builder.Services.AddScoped<IChapterRepository, EFChapterRepository>();
builder.Services.AddScoped<IKhachHangRepository, EFKhachHangRepository>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<MangaSmurfContext>().AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();


var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();
app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=BoTruyenManager}/{action=Index}/{id?}"
        );
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=BoTruyenManager}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BoTruyen}/{action=Index}/{id?}");

app.Run();
