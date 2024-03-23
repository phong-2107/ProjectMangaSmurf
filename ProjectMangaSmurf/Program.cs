using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MangaSmurfContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IboTruyenRepository, EFboTruyenRepository>();
builder.Services.AddScoped<IChapterRepository, EFChapterRepository>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BoTruyen}/{action=Index}/{id?}");

app.Run();
