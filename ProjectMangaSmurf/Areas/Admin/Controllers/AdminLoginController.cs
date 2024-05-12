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
                var user = await _staffRepository.GetByAccountAsyncStaff(Taikhoan);
                if (user != null && user.StaffRole == true) // Checking if user is an Admin
                {
                    var User = await _userRepository.GetByIdAsync(user.IdUser);
                    var claims = new List<Claim>
                    {
                        
                        new Claim(ClaimTypes.Name, User.UserName),
                        new Claim(ClaimTypes.Role, "true")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "AdminAuthScheme");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true 
                    };

                    await HttpContext.SignInAsync("AdminAuthScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

                    HttpContext.Session.SetString("AdminSession", User.UserName);  // Set a session variable specific to admins

                    return RedirectToAction("Index", "BoTruyenManager");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password or not authorized as admin");
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminAuthScheme");  // Clears the authentication cookie
            HttpContext.Session.Remove("AdminSession");         // Clears the specific admin session variable
            return RedirectToAction("Login", "AdminLogin");
        }

        public bool CheckPassword(string providedPassword, string storedHash)
        {
            return PasswordHasher.VerifyPassword(providedPassword, storedHash);
        }
    }
}
