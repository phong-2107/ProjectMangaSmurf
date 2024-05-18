using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models.ViewModels;
using ProjectMangaSmurf.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class AdminLoginController : Controller
    {
        private readonly ProjectDBContext _context;
        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;

        public AdminLoginController(ProjectDBContext context, IUserRepository userRepository, IStaffRepository staffRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _staffRepository = staffRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Taikhoan, string Matkhau)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = await _staffRepository.GetByAccountAsyncStaff(Taikhoan);
                if (nhanVien == null)
                {
                    ModelState.AddModelError("", "Invalid username or email or password");
                    return View();
                }

                if (nhanVien.StaffRole != true)
                {
                    ModelState.AddModelError("", "Not authorized as admin");
                    return View();
                }

                var user = nhanVien.IdUserNavigation;
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View();
                }

                // Kiểm tra mật khẩu
                if (user.Password == null || !CheckPassword(Matkhau.Trim(), user.Password))
                {
                    ModelState.AddModelError("", "Invalid password");
                    return View();
                }

                // Lấy danh sách quyền hạn của người dùng
                var userPermissions = await _context.StaffPermissionsDetails
                    .Where(p => p.IdUser == user.IdUser && p.Active == true)
                    .Select(p => p.IdPermissions)
                    .ToListAsync();

                // Chuyển danh sách quyền hạn thành chuỗi và lưu trữ trong session
                var permissions = string.Join(",", userPermissions);
                HttpContext.Session.SetString("UserPermissions", permissions);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? "UnknownUser"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.NameIdentifier, user.IdUser) // Thêm Claim để lưu trữ IdUser
                };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminAuthScheme");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync("AdminAuthScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetString("AdminSession", user.UserName ?? "UnknownUser");
                HttpContext.Session.SetString("AdminSessionId", user.IdUser ?? "UnknownUserID");

                return RedirectToAction("Index", "BoTruyenManager");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminAuthScheme");
            HttpContext.Session.Remove("AdminSession");
            HttpContext.Session.Remove("UserPermissions"); // Xóa quyền hạn khỏi session khi đăng xuất
            return RedirectToAction("Login", "AdminLogin");
        }

        public bool CheckPassword(string providedPassword, string storedHash)
        {
            return PasswordHasher.VerifyPassword(providedPassword, storedHash);
        }
    }
}
