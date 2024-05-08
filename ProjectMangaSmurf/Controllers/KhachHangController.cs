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
using Microsoft.AspNetCore.Http;
using ProjectMangaSmurf.Data;
namespace ProjectMangaSmurf.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly ICTBoTruyenRepository _cTBoTruyenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAvatarRepository _avatarRepository;
        private readonly ProjectDBContext _context;
        public KhachHangController(IKhachHangRepository khachHangRepository, IboTruyenRepository boTruyenRepository, IChapterRepository chapterRepository, ICTBoTruyenRepository cTBoTruyenRepository, IUserRepository userRepository, IAvatarRepository avatarRepository, ProjectDBContext db)
        {
            _khachhangrepository = khachHangRepository;
            _botruyenrepository = boTruyenRepository;
            _chapterrepository = chapterRepository;
            _cTBoTruyenRepository = cTBoTruyenRepository;
            _userRepository = userRepository;
            _avatarRepository = avatarRepository;
            _context = db;
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

        public IActionResult login()
        {
            return View();
        }

        public async Task<ActionResult> Account()
        {

            var kh = await _khachhangrepository.GetByAccountAsync(HttpContext.Session.GetString("TK"));
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
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
                var emailClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var nameClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                HttpContext.Session.SetString("Email", emailClaim);
                HttpContext.Session.SetString("TK", nameClaim);
                var find = await _khachhangrepository.GetByEmailAsync(HttpContext.Session.GetString("Email"));
                if (find == null)
                {
                    var avatar = await _avatarRepository.RandomAvatarAsync();
                    User user = new User();
                    user.IdUser = _khachhangrepository.GenerateCustomerId();
                    user.UserName = HttpContext.Session.GetString("TK");
                    user.Password = RandomPass();
                    user.Email = HttpContext.Session.GetString("Email");
                    user.TimeCreated = DateTime.Now;
                    user.TimeUpdated = DateTime.Now;
                    user.Active = true;

                    KhachHang kh = new KhachHang();
                    kh.IdUser = user.IdUser;
                    kh.GoogleAccount = HttpContext.Session.GetString("Email");
                    kh.ActivePremium = false;
                    kh.IdAvatar = avatar.IdAvatar;
                    kh.TicketSalary = 0;
                    kh.ActiveStats = 1;

                    await _userRepository.AddAsyncKH(user, kh);
                    HttpContext.Session.SetString("Img", avatar.AvatarContent);
                    HttpContext.Session.SetString("IdKH", kh.IdUser);
                    return RedirectToAction("Infor", "KhachHang");
                }
                else
                {
                    var avatar = await _avatarRepository.GetByIdAsync(find.IdAvatar);
                    HttpContext.Session.SetString("Img", avatar.AvatarContent);
                    HttpContext.Session.SetString("IdKH", find.IdUser);
                    return RedirectToAction("Index", "BoTruyen");
                }
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
            HttpContext.Session.Remove("IdKH");
            HttpContext.Session.Remove("Img");
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index", "BoTruyen");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Taikhoan, string Matkhau)
        {
            if (ModelState.IsValid)
            {


                var kh = await _khachhangrepository.GetByAccountAsync(Taikhoan);
                if (kh != null)
                {
                    var User = await _userRepository.GetByIdAsync(kh.IdUser);
                    var check = PasswordHasher.VerifyPassword(Matkhau.Trim(), User.Password);
                    if (check)
                    {
                        var avatar = await _avatarRepository.GetByIdAsync(kh.IdAvatar);
                        HttpContext.Session.SetString("TK", kh.IdUserNavigation.UserName);
                        HttpContext.Session.SetString("Img", avatar.AvatarContent);
                        HttpContext.Session.SetString("IdKH", kh.IdUser);
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
                    ModelState.AddModelError(string.Empty, "Tài Khoản của bạn chưa chính xác");
                    return View();
                }

            }
            ModelState.AddModelError(string.Empty, "Tài khoản không hợp lệ");
            return View();
        }

        public IActionResult LoginPopup(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> LoginPopup(string Taikhoan, string Matkhau, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var kh = await _khachhangrepository.GetByAccountAsync(Taikhoan);
                if (kh != null)
                {
                    var check = PasswordHasher.VerifyPassword(Matkhau.Trim(), kh.IdUserNavigation.Password.Trim());
                    if (check)
                    {
                        // Set session variables
                        HttpContext.Session.SetString("TK", kh.IdUserNavigation.UserName);
                        HttpContext.Session.SetString("IdKH", kh.IdUser);

                        // Check if the returnUrl is not null and is a local URL to prevent Open Redirect vulnerabilities
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        // If no valid returnUrl, redirect to a default page
                        return RedirectToAction("Index", "BoTruyen");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Mật khẩu không đúng");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản của bạn không đúng");
                    return View();
                }
            }
            ModelState.AddModelError(string.Empty, "Thông tin tài khoản không hợp lệ");
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Taikhoan, string Email, string matKhau)
        {
            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existingUser = await _khachhangrepository.GetByEmailAsync(Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Email này đã tồn tại";
                return View();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Tạo người dùng mới
                    var avatar = await _avatarRepository.RandomAvatarAsync();
                    User user = new User
                    {
                        IdUser = _khachhangrepository.GenerateCustomerId(),
                        UserName = Taikhoan.Trim(),
                        Password = PasswordHasher.HashPassword(matKhau),
                        Email = Email.Trim(),
                        TimeCreated = DateTime.Now,
                        TimeUpdated = DateTime.Now,
                        Active = true
                    };

                    await _userRepository.AddAsync(user);

                    // Tạo Khách Hàng mới
                    KhachHang kh = new KhachHang
                    {
                        IdUser = user.IdUser,
                        ActivePremium = false,
                        IdAvatar = avatar.IdAvatar,
                        TicketSalary = 0,
                        ActiveStats = 1
                    };

                    await _khachhangrepository.AddAsync(kh);
                    transaction.Commit();
                    HttpContext.Session.SetString("Img", avatar.AvatarContent);
                    HttpContext.Session.SetString("TK", user.UserName);
                    HttpContext.Session.SetString("IdKH", user.IdUser);

                    return RedirectToAction("infor", "KhachHang");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Có lỗi xảy ra khi đăng ký: " + ex.Message;
                    return View();
                }
            }
        }
        public async Task<IActionResult> Infor()
        {
            var IdKH = HttpContext.Session.GetString("IdKH");
            var tk = HttpContext.Session.GetString("TK");
            ViewBag.Username = tk;
            var kh = await _khachhangrepository.GetByIdAsync(IdKH);
            var ava = await _avatarRepository.GetByIdAsync(kh.IdAvatar);
            ViewBag.Url = ava.AvatarContent;
            var user = await _userRepository.GetByIdAsync(kh.IdUser);
            ViewBag.FullName = user.FullName;

            var listAvatar = await _avatarRepository.GetAllAsync();
            ViewBag.Avatars = listAvatar;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Infor(string Username, string FullName, int Gender, DateOnly date)
        {
            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                return View();
            }

            var Us = await _userRepository.GetByIdAsync(HttpContext.Session.GetString("IdKH"));
            if (Us == null)
            {
                return View();
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Us.UserName = Username;
                    Us.FullName = FullName;
                    Us.Birth = date;
                    Us.Gender = (byte)Gender;
                    Us.TimeUpdated = DateTime.Now;
                    await _userRepository.UpdateAsync(Us);
                    transaction.Commit();
                    HttpContext.Session.SetString("TK", Us.UserName);
                    return RedirectToAction("Index", "BoTruyen");
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi và rollback transaction
                    transaction.Rollback();
                    ViewBag.Error = "Có lỗi xảy ra khi thêm thông tin: " + ex.Message;
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(string id)
        {
            if (ModelState.IsValid)
            {
                var kh = await _khachhangrepository.GetByAccountAsync(HttpContext.Session.GetString("TK"));
                if (kh == null)
                {
                    return RedirectToAction("Infor", "KhachHang");
                }

                kh.IdAvatar = id;
                await _khachhangrepository.UpdateAsync(kh);
                var avatar = await _avatarRepository.GetByIdAsync(id);
                HttpContext.Session.SetString("Img", avatar.AvatarContent);
                return RedirectToAction("Infor", "KhachHang");
            }
            return RedirectToAction("Infor", "KhachHang");
        }

        public async Task<IActionResult> Following(string id)
        {
            var kh = await _khachhangrepository.GetByAccountAsync(id);
            var list = await _cTBoTruyenRepository.GetAllAsyncFollowByID(kh.IdUser);
            return View(list);
        }

        public async Task<IActionResult> History(string id)
        {
            var kh = await _khachhangrepository.GetByAccountAsync(id);
            var list = await _cTBoTruyenRepository.GetAllAsyncHistoryByID(kh.IdUser);
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id, string stt)
        {
            if (ModelState.IsValid)
            {
                var botruyen = await _botruyenrepository.GetByIdAsync(id);
                var kh = await _khachhangrepository.GetByIdAsync(HttpContext.Session.GetString("IdKH"));

                botruyen.TkTheodoi = botruyen.TkTheodoi + 1;
                await _botruyenrepository.UpdateAsync(botruyen);

                var ctbo = _cTBoTruyenRepository.GetByIdAsync(kh.IdUser, id);
                if (await ctbo == null)
                {
                    CtBoTruyen ct = new CtBoTruyen();
                    ct.IdUser = kh.IdUser;
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
                var kh = await _khachhangrepository.GetByIdAsync(HttpContext.Session.GetString("IdKH"));

                botruyen.TkTheodoi = botruyen.TkTheodoi + 1;
                await _botruyenrepository.UpdateAsync(botruyen);

                var ctbo = _cTBoTruyenRepository.GetByIdAsync(kh.IdUser, id);
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
            return RedirectToAction("History", "KhachHang", new { id = kh });
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
                var idkh = HttpContext.Session.GetString("IdKH");
                var kh = await _khachhangrepository.GetByIdAsync(idkh);
                var botruyen = _cTBoTruyenRepository.GetByIdAsync(kh.IdUser, id);
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
            var khach = _khachhangrepository.GetByAccountAsync(idkh);
            if (await khach != null)
            {
                //User user = new User();
                //user.IdUser = _khachhangrepository.GenerateCustomerId();
                //user.UserName = HttpContext.Session.GetString("TK");
                //user.Email = HttpContext.Session.GetString("Email");
                //user.TimeCreated = DateTime.Now;
                //user.TimeUpdated = DateTime.Now;
                //user.Active = true;
                //await _userRepository.AddAsync(user);

                var kh = await khach;
                kh.GoogleAccount = Email;

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
            var khach = await _khachhangrepository.GetByAccountAsync(idkh);
            if (khach != null)
            {
                if (khach.GoogleAccount == null)
                {
                    var check = PasswordHasher.VerifyPassword(currentPassword.Trim(), khach.IdUserNavigation.Password.Trim());
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
                khach.IdUserNavigation.Password = PasswordHasher.HashPassword(confirmNewPassword);
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
            var khach = await _khachhangrepository.GetByAccountAsync(idkh);
            if (khach != null)
            {
                if (khach.GoogleAccount == null)
                {
                    khach.FacebookAccount = LienketFb;
                    khach.GoogleAccount = LienketGg;
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
            var khach = await _khachhangrepository.GetByAccountAsync(idkh);
            if (khach != null)
            {
                if (khach.GoogleAccount == null)
                {
                    khach.FacebookAccount = LienketFb;
                    khach.GoogleAccount = LienketGg;
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
