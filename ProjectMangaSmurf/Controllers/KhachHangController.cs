using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text;
using System;
namespace ProjectMangaSmurf.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly ICTBoTruyenRepository _cTBoTruyenRepository;
        public KhachHangController( IKhachHangRepository khachHangRepository, IboTruyenRepository boTruyenRepository, IChapterRepository chapterRepository, ICTBoTruyenRepository cTBoTruyenRepository)
        {
            _khachhangrepository = khachHangRepository;
            _botruyenrepository = boTruyenRepository;
            _chapterrepository = chapterRepository;
            _cTBoTruyenRepository = cTBoTruyenRepository;
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

        public async Task<ActionResult> Account()
        {

            var kh = await _khachhangrepository.GetByIdAsync(HttpContext.Session.GetString("TK"));
            if (kh != null)
            {
                return View(kh);
            }
            return View();
        }

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public string RandomPass()
        {
            Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < 25; i++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }

        public async Task<ActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result?.Principal != null)
            {
                // Lấy thông tin từ claims
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
                var emailClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var nameClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                HttpContext.Session.SetString("Email", emailClaim);
                HttpContext.Session.SetString("TK", nameClaim);

                var find = _khachhangrepository.GetByEmailAsync(HttpContext.Session.GetString("Email"));
                if(await find == null)
                {
                    KhachHang kh = new KhachHang();
                    kh.IdKh = _khachhangrepository.GenerateCustomerId();
                    kh.Email = HttpContext.Session.GetString("Email");
                    kh.Taikhoan = HttpContext.Session.GetString("TK");
                    kh.LienketGg = HttpContext.Session.GetString("TK");
                    kh.Matkhau = RandomPass();
                    kh.TtPremium = false;
                    kh.Active = true;
                    _khachhangrepository.AddAsync(kh);
                }
                return RedirectToAction("Index", "BoTruyen");
            }
            else
            {
                return Challenge(GoogleDefaults.AuthenticationScheme);
            }
        }
        public async Task<ActionResult> LogoutGoogle()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("TK");
            return RedirectToAction("Index", "BoTruyen");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Taikhoan, string Matkhau)
        {
            if (ModelState.IsValid)
            {
                var kh = await _khachhangrepository.GetByIdAsync(Taikhoan);
                if (kh != null)
                {
                    var check = PasswordHasher.VerifyPassword(Matkhau.Trim(), kh.Matkhau.Trim());
                    if (check)
                    {
                        HttpContext.Session.SetString("TK", kh.Taikhoan);
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
                var find = _khachhangrepository.GetByEmailAsync(Email);
                if( await find != null)
                {
                    ViewBag.Error = "Email này đã tồn tại";
                    return View();
                }
                else
                {
                    KhachHang kh = new KhachHang();
                    kh.IdKh = _khachhangrepository.GenerateCustomerId();
                    kh.Email = Email.Trim();
                    kh.Taikhoan = Taikhoan.Trim();
                    kh.TtPremium = false;
                    kh.Active = true;
                    kh.Matkhau = PasswordHasher.HashPassword(matKhau);
                    await _khachhangrepository.AddAsync(kh);
                    HttpContext.Session.SetString("TK", kh.Taikhoan);
                    return RedirectToAction("Index", "BoTruyen");
                }
                
            }
            return View();
        }

        public async Task<IActionResult> Following(string id)
        {
            var kh = await _khachhangrepository.GetByIdAsync(id);
            var list = await _cTBoTruyenRepository.GetAllAsyncFollowByID(kh.IdKh);
            return View(list);
        }

        public async Task<IActionResult> History(string id)
        {
            var kh = await _khachhangrepository.GetByIdAsync(id);
            var list = await _cTBoTruyenRepository.GetAllAsyncHistoryByID(kh.IdKh);
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id, string stt)
        {
            if (ModelState.IsValid)
            {
                var botruyen = await _botruyenrepository.GetByIdAsync(id);
                var kh = await _khachhangrepository.GetByIdAsync(HttpContext.Session.GetString("TK"));

                botruyen.TkTheodoi = botruyen.TkTheodoi + 1;
                await _botruyenrepository.UpdateAsync(botruyen);

                var ctbo = _cTBoTruyenRepository.GetByIdAsync(kh.IdKh, id);
                if( await ctbo == null)
                {
                    CtBoTruyen ct = new CtBoTruyen();
                    ct.IdKh = kh.IdKh;
                    ct.IbBo = id;
                    ct.Theodoi = true;
                    ct.DanhGia = 0;
                    ct.LsMoi = stt;
                    await _cTBoTruyenRepository.AddAsync(ct);
                }
                else
                {
                    var ctbotruyen = await ctbo;
                    ctbotruyen.Theodoi = true;
                    await _cTBoTruyenRepository.UpdateAsync(ctbotruyen);
                }
                
            }
            return RedirectToAction("Chapter", "BoTruyen", new { id = id, stt = stt });
        }

        [HttpPost]
        public async Task<IActionResult> UnFollow(string id, string stt)
        {
            if (ModelState.IsValid)
            {
                var botruyen = await _botruyenrepository.GetByIdAsync(id);
                var kh = await _khachhangrepository.GetByIdAsync(HttpContext.Session.GetString("TK"));

                botruyen.TkTheodoi = botruyen.TkTheodoi + 1;
                await _botruyenrepository.UpdateAsync(botruyen);

                var ctbo = _cTBoTruyenRepository.GetByIdAsync(kh.IdKh, id);
                if (await ctbo != null)
                {
                    var ctbotruyen = await ctbo;
                    ctbotruyen.Theodoi = false;
                    await _cTBoTruyenRepository.UpdateAsync(ctbotruyen);
                }
                
            }
            return RedirectToAction("Chapter", "BoTruyen", new { id = id, stt = stt });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFollow(string id, string truyen)
        {
            var kh = HttpContext.Session.GetString("TK");
            if (ModelState.IsValid)
            {
                var botruyen = await _cTBoTruyenRepository.GetByIdAsync(id, truyen);
                if (botruyen != null)
                {
                    botruyen.Theodoi = false;
                    await _cTBoTruyenRepository.UpdateAsync(botruyen);
                }

            }
            return RedirectToAction("History", "KhachHang", new { id = kh});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHistory(string id, string truyen)
        {
            var kh = HttpContext.Session.GetString("TK");
            if (ModelState.IsValid)
            {
                var botruyen = await _cTBoTruyenRepository.GetByIdAsync(id, truyen);
                if (botruyen != null)
                {
                    botruyen.LsMoi = "-1";
                    await _cTBoTruyenRepository.UpdateAsync(botruyen);
                }
            }
            return RedirectToAction("History", "KhachHang", new { id = kh });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRating(string id, string rate)
        {
            if (ModelState.IsValid)
            {
                var idkh = HttpContext.Session.GetString("TK");
                var kh = await _khachhangrepository.GetByIdAsync(idkh);
                var botruyen =  _cTBoTruyenRepository.GetByIdAsync(kh.IdKh, id);
                if (await botruyen != null)
                {
                    var bt = await botruyen;
                    bt.DanhGia = (byte)int.Parse(rate);
                    await _cTBoTruyenRepository.UpdateAsync(bt);
                }
            }
            return RedirectToAction("CtBoTruyen", "BoTruyen", new { id = id });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateInfor(string Taikhoan, string TenKh, string Sdt, string Email)
        {
                var idkh = HttpContext.Session.GetString("TK");
                var khach =  _khachhangrepository.GetByIdAsync(idkh);
                if (await khach != null)
                {
                    var kh = await khach;
                    kh.TenKh = TenKh;
                    kh.Sdt = Sdt;
                    kh.Email = Email;
                    kh.Taikhoan = Taikhoan;
                    await _khachhangrepository.UpdateAsync(kh);

                    TempData["info"] = "Cập nhật thông tin thành công";
                    return RedirectToAction("Account");
                }
            
            return RedirectToAction("Account", "KhachHang");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePass(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var idkh = HttpContext.Session.GetString("TK");
            var khach = await _khachhangrepository.GetByIdAsync(idkh);
            if ( khach != null)
            {
                if(khach.LienketGg == null)
                {
                    var check = PasswordHasher.VerifyPassword(currentPassword.Trim(), khach.Matkhau.Trim());
                    if (!check)
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không trùng khớp.");
                        return View("Account");
                    }
                    if (newPassword != confirmNewPassword)
                    {
                        ModelState.AddModelError("", "The new passwords do not match.");
                        return View("Account");
                    }
                }
                else
                {
                    if (newPassword != confirmNewPassword)
                    {
                        ModelState.AddModelError("", "The new passwords do not match.");
                        return View("Account");
                    }
                }
                khach.Matkhau = PasswordHasher.HashPassword(confirmNewPassword);
                await _khachhangrepository.UpdateAsync(khach);

                TempData["info"] = "Cập nhật thành công mật khẩu";
                return RedirectToAction("Account");
            }

            return RedirectToAction("Account");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSocial(string LienketFb, string LienketGg)
        {
            var idkh = HttpContext.Session.GetString("TK");
            var khach = await _khachhangrepository.GetByIdAsync(idkh);
            if (khach != null)
            {
                if (khach.LienketGg == null)
                {
                    khach.LienketFb = LienketFb;
                    khach.LienketGg = LienketGg;
                }
                else
                {
                        ModelState.AddModelError("LienketGg", "Tài khỏan của bạn đã liên kết với một Email trước đó");
                        return View("Account");
                }
                await _khachhangrepository.UpdateAsync(khach);
                TempData["info"] = "Cập nhật Social Thành Công!!!";
            }

            return RedirectToAction("Account");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConnect(string LienketFb, string LienketGg)
        {
            var idkh = HttpContext.Session.GetString("TK");
            var khach = await _khachhangrepository.GetByIdAsync(idkh);
            if (khach != null)
            {
                if (khach.LienketGg == null)
                {
                    khach.LienketFb = LienketFb;
                    khach.LienketGg = LienketGg;
                }
                else
                {
                    ModelState.AddModelError("LienketGg", "Tài khỏan của bạn đã liên kết với một Email trước đó");
                    return View("Account");
                }
                await _khachhangrepository.UpdateAsync(khach);
                TempData["info"] = "Cập nhật Social Thành Công!!!";
            }

            return RedirectToAction("Account");
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

    }
}
