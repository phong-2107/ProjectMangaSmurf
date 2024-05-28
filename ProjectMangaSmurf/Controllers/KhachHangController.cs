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
using PayPal.Api;
using System.util;
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
        private readonly IHopdongRepository _hopdongRepository;
        private readonly IEmailService _emailRepository;
        private readonly ProjectDBContext _context;
        public KhachHangController(IKhachHangRepository khachHangRepository, 
                                IboTruyenRepository boTruyenRepository, 
                                IChapterRepository chapterRepository, 
                                ICTBoTruyenRepository cTBoTruyenRepository, 
                                IUserRepository userRepository, 
                                IAvatarRepository avatarRepository, 
                                IHopdongRepository hopdongRepository,
                                IEmailService emailService,
                                ProjectDBContext db)
        {
            _khachhangrepository = khachHangRepository;
            _botruyenrepository = boTruyenRepository;
            _chapterrepository = chapterRepository;
            _cTBoTruyenRepository = cTBoTruyenRepository;
            _userRepository = userRepository;
            _avatarRepository = avatarRepository;
            _hopdongRepository = hopdongRepository;
            _emailRepository = emailService;
            _context = db;
        }

        #region Giải quyết logic
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
        #endregion


        #region Action 
        public IActionResult login()
        {

            return View();
        }

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
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

                    var payment = await _hopdongRepository.GetByIdAsync(find.IdUser);
                    if (payment != null)
                    {
                        DateTimeOffset todayOffset = new DateTimeOffset(DateTime.Today, TimeZoneInfo.Local.GetUtcOffset(DateTime.Today));
                        if (payment.ExpiresTime >= todayOffset && find.TicketSalary <= 0)
                        {
                            find.ActivePremium = false;
                            await _khachhangrepository.UpdateAsync(find);
                        }
                    }
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
            HttpContext.Session.Remove("date");
            HttpContext.Session.Remove("OrderId");
            HttpContext.Session.Remove("Message");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Phone");
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

                        var payment = await _hopdongRepository.GetByIdAsync(User.IdUser);
                        if(payment != null)
                        {
                            DateTimeOffset todayOffset = new DateTimeOffset(DateTime.Today, TimeZoneInfo.Local.GetUtcOffset(DateTime.Today));
                            if (payment.ExpiresTime >= todayOffset && kh.TicketSalary <= 0)
                            {
                                kh.ActivePremium = false;
                                await _khachhangrepository.UpdateAsync(kh);
                            }
                        }
                        return Json(new { success = true });
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
                        HttpContext.Session.SetString("TK", kh.IdUserNavigation.UserName);
                        HttpContext.Session.SetString("IdKH", kh.IdUser);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
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

        public string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
            }
            return template;
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email, string Code)
        {
            if (ModelState.IsValid)
            {
                var kh = await _userRepository.GetByEmailAsync(Email);
                if (kh != null)
                {
                    if(Code != "0")
                    {
                        if(Code == HttpContext.Session.GetString("CODE"))
                        {
                            HttpContext.Session.Remove("CODE");
                            return Json(new { success = true });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Mã Không chính xác" });
                        }
                    }
                    else
                    {
                        Random random = new Random();
                        var ran = random.Next(100000, 1000000);

                        var email = Email;
                        var subject = "Reset Password Mangasmurf";
                        var message = "Khôi phục mật khẩu ";

                        string templatePath = "ForgotPassword.html";
                        string emailTemplate = await _emailRepository.ReadTemplateAsync(templatePath);

                        var placeholders = new Dictionary<string, string>
                    {
                        { "CODE", ran.ToString()},
                    };

                        string emailBody = ReplacePlaceholders(emailTemplate, placeholders);
                        await _emailRepository.SendEmailTemplateAsync(email, subject, message, emailBody);

                        HttpContext.Session.SetString("CODE", ran.ToString());
                        HttpContext.Session.SetString("MailRecovery", Email);
                        return Json(new { success = false, message = "CODE đã được gửi đến Email của bạn" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Email chưa liên kết với tài khoản" });
                }

            }

            return Json(new { success = false, message = "Email không hợp lệ" });
        }

        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRecovery(string newpass, string repass)
        {
            if (ModelState.IsValid)
            {
                var Email = HttpContext.Session.GetString("MailRecovery");
                var kh = await _userRepository.GetByEmailAsync(Email);
                if (kh != null)
                {
                    if (repass == newpass)
                    {
                        kh.Password = PasswordHasher.HashPassword(newpass);
                        await _userRepository.UpdateAsync(kh);

                        HttpContext.Session.Remove("MailRecovery");
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Mật khẩu không trùng khớp" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Thông tin lỗi" });
                }

            }

            return Json(new { success = false, message = "có lỗi trong quá trình nhập" });
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
            //ViewBag.date = user.Birth;
            ViewBag.sex = user.Gender;
            var listAvatar = await _avatarRepository.GetAllAsync();
            ViewBag.Avatars = listAvatar;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Infor(string Username, string FullName, int Gender, DateOnly date)
        {
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
                    return Json(new { success = true });
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
                var botruyen = await _cTBoTruyenRepository.GetByIdAsync(kh.IdUser, id);
                if ( botruyen != null)
                {
                    var bt = botruyen;
                    bt.DanhGia = (byte)int.Parse(rate);
                    await _cTBoTruyenRepository.UpdateAsync(bt);

                    var truyen = await _botruyenrepository.GetByIdAsync(id);
                    var rating = await _cTBoTruyenRepository.CountAllAsyncByBoTruyen(id);
                    truyen.TkDanhgia = rating;
                    await _botruyenrepository.UpdateAsync(truyen);
                }
                else
                {
                    CtBoTruyen ct = new CtBoTruyen();
                    ct.IdUser = idkh;
                    ct.IbBo = id;
                    ct.Theodoi = false;
                    ct.DanhGia = (byte)int.Parse(rate);
                    ct.LsMoi = "0";
                    await _cTBoTruyenRepository.AddAsync(ct);

                    var truyen = await _botruyenrepository.GetByIdAsync(id);
                    var rating = await _cTBoTruyenRepository.CountAllAsyncByBoTruyen(id);
                    truyen.TkDanhgia = rating;
                    await _botruyenrepository.UpdateAsync(truyen);
                }
                return Json(new { success = true, message = "Đánh giá thành công" });
            }
            return Json(new { success = false, message = "Thông tin lỗi" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInfor(string Taikhoan, string TenKh, string Sdt, string birthdate)
        {
            var idkh = HttpContext.Session.GetString("TK");
            var khach = _khachhangrepository.GetByAccountAsync(idkh);
            if (await khach != null)
            {
                var kh = await khach;

                var user = await _userRepository.GetByIdAsync(kh.IdUser);
                if (user != null)
                {
                    user.UserName = Taikhoan;
                    user.FullName = TenKh;
                    user.Phone = Sdt;
                    user.Birth = DateOnly.Parse(birthdate);
                    await _userRepository.UpdateAsync(user);
                }

                TempData["info"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Account");
            }

            return RedirectToAction("Account", "KhachHang");
        }


        public async Task<ActionResult> Account()
        {
            var kh = await _khachhangrepository.GetByIdAsync(HttpContext.Session.GetString("IdKH"));
            var value = TempData["info"] as string;
            ViewBag.info = value;
            if (kh != null)
            {
                var user = await _userRepository.GetByIdAsync(kh.IdUser);

                ViewBag.user = user;

                var listPay = await _hopdongRepository.GetPaymentByIdAsync(kh.IdUser);
                ViewBag.pays = listPay;
                return View(kh);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePass(string Email, string currentPassword, string newPassword, string confirmNewPassword)
        {
            var idkh = HttpContext.Session.GetString("TK");
            var khach = await _khachhangrepository.GetByAccountAsync(idkh);
            if (khach != null)
            {
                var user = await _userRepository.GetByIdAsync(khach.IdUser);
                if (Email == user.Email)
                {
                    if(khach.GoogleAccount != null)
                    {
                        if (newPassword != confirmNewPassword)
                        {
                            TempData["info"] = "The new passwords do not match.";
                            return RedirectToAction("Account");
                        }
                        khach.IdUserNavigation.Password = PasswordHasher.HashPassword(confirmNewPassword);
                        await _khachhangrepository.UpdateAsync(khach);
                        TempData["info"] = "Cập nhật mật khẩu thành công";
                        return RedirectToAction("Account");
                    }
                    else
                    {
                        var check = PasswordHasher.VerifyPassword(currentPassword.Trim(), user.Password);
                        if (!check)
                        {
                            TempData["info"] = "Mật khẩu cũ không trùng khớp.";
                            return RedirectToAction("Account");
                        }
                        if (newPassword != confirmNewPassword)
                        {
                            TempData["info"] = "The new passwords do not match.";
                            return RedirectToAction("Account");
                        }
                        khach.IdUserNavigation.Password = PasswordHasher.HashPassword(confirmNewPassword);
                        await _khachhangrepository.UpdateAsync(khach);
                        TempData["info"] = "Cập nhật mật khẩu thành công";
                        return RedirectToAction("Account");
                    }
                }
                else
                {
                    TempData["info"] = "Email cuả bạn không trùng khớp";
                    return RedirectToAction("Account");
                }
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
        #endregion
    }
}
