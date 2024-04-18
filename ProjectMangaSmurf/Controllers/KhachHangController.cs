using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace ProjectMangaSmurf.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangRepository _khachhangrepository;
        public KhachHangController( IKhachHangRepository khachHangRepository)
        {
            _khachhangrepository = khachHangRepository;
        }

        public IActionResult Index()
        {
            var kh = HttpContext.Session.GetObjectFromJson<KhachHang>("kh") ?? new KhachHang();
            ViewBag.Khachhang = kh;
            return PartialView("_MenuPartial", kh);
        }
        public bool checkpass(string pass, string passKh)
        {
            if (pass != null && passKh != null)
            {
                if (pass.Trim().Equals(passKh))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Taikhoan, string matKhau)
        {
            if (ModelState.IsValid)
            {
                var kh = await _khachhangrepository.GetByIdAsync(Taikhoan);
                if (kh != null)
                {
                    var check = PasswordHasher.VerifyPassword(matKhau.Trim(), kh.Matkhau.Trim());
                    if (check)
                    {
                        HttpContext.Session.SetObjectAsJson("kh", kh);
                        return RedirectToAction("Index", "BoTruyen");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "mật khẩu không đúng");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tai Khoan của bạn không đúng");
                    return View();
                }

            }
            ModelState.AddModelError(string.Empty, "tai khoan kh hop le");
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Taikhoan,string Email, string matKhau)
        {
            if (ModelState.IsValid)
            {
                KhachHang kh = new KhachHang();
                kh.IdKh = _khachhangrepository.GenerateCustomerId();
                kh.Email = Email.Trim();
                kh.Taikhoan = Taikhoan.Trim();
                kh.TtPremium = false;
                kh.Active = true;
                kh.Matkhau = PasswordHasher.HashPassword(matKhau);
                await _khachhangrepository.AddAsync(kh);
                HttpContext.Session.SetObjectAsJson("kh", kh);
                return RedirectToAction("Index", "BoTruyen");
            }
            return View();
        }


        public IActionResult Success()
        {
            KhachHang registeredKhachHang = TempData["RegisteredKhachHang"] as KhachHang;
            if (registeredKhachHang == null)
            {
                return RedirectToAction("Index", "Register"); 
            }
            return View(registeredKhachHang);
        }

        public IActionResult Contact()
        {
            return View();
        }

        //public IActionResult account()
        //{
        //    var kh = HttpContext.Session.GetObjectFromJson<KhachHang>("kh") ?? new KhachHang();
        //    ViewBag.KH = kh;
        //    return View();
        //}


    }
}
