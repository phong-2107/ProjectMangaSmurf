
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectMangaSmurf.Areas.Common.Repository;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Areas.Common.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "603767058702-cli1i4notmjpfqo2jg2s7t7001o6i0i0.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
    googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
    googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/KhachHang/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
})
.AddCookie("AdminAuthScheme", options =>
{
    options.LoginPath = new PathString("/Admin/AdminLogin/Login");
    options.AccessDeniedPath = new PathString("/Admin/AdminLogin/AccessDenied");
    options.Cookie.Name = "AdminAuthCookie";
});



builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//    options.Cookie.Name = ".AspNetCore.Admin.Session";
//});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = ".AspNetCore.Admin.Session";  
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".AspNetCore.Main.Session";  
});


services.AddControllersWithViews();

services.AddControllersWithViews()
    .AddCookieTempDataProvider(); 
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
services.AddSession();

var connectionString = builder.Configuration.GetConnectionString("ProjectDBContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'ProjectDBContextConnection' " +
    "not found.");

builder.Services.AddDbContext<ProjectDBContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddRazorPages();

builder.Services.AddScoped<IUser,EFUser>();
builder.Services.AddScoped<CustomAuthenticationService>();
builder.Services.AddScoped<IStaffRepository,EFStaffRepository>();
builder.Services.AddScoped<IService, EFService>();
builder.Services.AddScoped<IComicTypeRepository, EFComicTypeRepository>();
builder.Services.AddScoped<IAuthorRepository, EFAuthorRepository>();
builder.Services.AddScoped<IboTruyenRepository, EFboTruyenRepository>();
builder.Services.AddScoped<ICTBoTruyenRepository, EFCTBoTruyenRepository>();
builder.Services.AddScoped<ILoaiTruyenRepository, EFLoaiTruyenRepository>();
builder.Services.AddScoped<ITacGiaRepository, EFTacGiaRepository>();
builder.Services.AddScoped<IChapterRepository, EFChapterRepository>();
builder.Services.AddScoped<IKhachHangRepository, EFKhachHangRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IAvatarRepository, EFAvatarRepository>();
builder.Services.AddScoped<IHopdongRepository, EFHopDongRepository>();
builder.Services.AddScoped<IWebMediaRepository, EFWebMediaRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

//======================= Payment =======================
builder.Services.AddScoped<IVNPayRepository, EFVNPayRepository>();

//======================= Manager =======================
builder.Services.AddScoped<IStaffRepository, EFStaffRepository>();

//======================= Recommendation Book =======================
builder.Services.AddScoped<IBookRecommendationRepository, EFBookRecommendationRepository>();


//services.AddScoped<RBACAuthorizeAttribute>();

//services.AddControllersWithViews(options =>
//{
//    options.Filters.AddService<RBACAuthorizeAttribute>();
//});


builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("FlaskAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000"); 
});


var app = builder.Build();


app.UseRouting();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();


app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=BoTruyenManager}/{action=Index}/{id?}"
    ).RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = "AdminAuthScheme" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=BoTruyen}/{action=Index}/{id?}");
});
app.Run();
