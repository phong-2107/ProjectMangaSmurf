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
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        public KhachHangController(IboTruyenRepository botruyenrepository, IKhachHangRepository khachHangRepository)
        {
            _botruyenrepository = botruyenrepository;
            _khachhangrepository = khachHangRepository;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
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
                        ViewBag.Title = kh.Taikhoan;
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
            return View();
            //var check = checkpass(matKhau, khachHang.Matkhau);
            //if(check)
            //{
            //    
            //}
            //else
            //{
            //    

            //}
            //if (kh != null)
            //{
            //    var result = await

            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("Index", "BoTruyen", new { id = Taikhoan.Trim() });
            //    }
            //}
            //ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác.");
            //return View();
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
                return RedirectToAction("Index", "BoTruyen");
            }
            return View();
        }


        public IActionResult Success()
        {
            KhachHang registeredKhachHang = TempData["RegisteredKhachHang"] as KhachHang;
            if (registeredKhachHang == null)
            {
                // Xử lý trường hợp không có đối tượng KhachHang trong TempData
                return RedirectToAction("Index", "Register"); 
            }

            // Truyền đối tượng KhachHang vừa đăng ký vào view Success
            return View(registeredKhachHang);
        }

    }
}
