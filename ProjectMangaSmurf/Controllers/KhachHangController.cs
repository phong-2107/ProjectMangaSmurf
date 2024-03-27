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


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                khachHang.Matkhau = PasswordHasher.HashPassword(khachHang.Matkhau);
                khachHang.IdKh = _khachhangrepository.GenerateCustomerId();
                khachHang.TtPremium = true;
                khachHang.Active = true;

                await _khachhangrepository.AddAsync(khachHang);
                TempData["RegisteredKhachHang"] = khachHang;
                return RedirectToAction("Success", "Register"); 
            }
            return View(khachHang);
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
