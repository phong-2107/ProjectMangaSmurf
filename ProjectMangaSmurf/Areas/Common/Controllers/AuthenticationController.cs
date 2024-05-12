
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Areas.Common.Services;
using ProjectMangaSmurf.Models.ViewModels;

namespace ProjectMangaSmurf.Areas.Common.Controllers
{

    public class AuthenticationController : Controller
    {
        private CustomAuthenticationService _authService;

        public AuthenticationController(CustomAuthenticationService authService)
        {
            _authService = authService; // Assume this service handles the authentication logic
        }

        // GET: Authentication/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Authentication/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _authService.Login(model.Username, model.Password)) // Note the 'await' keyword
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "BoTruyenManager", new { area = "Admin" });
                }
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        // GET: Authentication/Logout
        public ActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login");
        }
    }

}
