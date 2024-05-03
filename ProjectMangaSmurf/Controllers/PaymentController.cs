using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;


namespace ProjectMangaSmurf.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVNPayRepository _vnPayservice;
        private readonly IHopdongRepository hopdongRepository;
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly IEmailService _emailRepository;
        public PaymentController(IKhachHangRepository khachHangRepository, IVNPayRepository vnPayservice, IHopdongRepository hopdong, IEmailService emailRepository)
        {
            _khachHangRepository = khachHangRepository;
            hopdongRepository = hopdong;
            _vnPayservice = vnPayservice;
            _emailRepository = emailRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Checkout(double Money, string Name, string phone, int OrderId)
        {
            if (ModelState.IsValid)
            {
                var vnPayModel = new VnPaymentRequestModel();
                vnPayModel.Amount = Money;
                vnPayModel.CreatedDate = DateTime.Now;
                vnPayModel.Description = $"{Name} {phone}";
                vnPayModel.FullName = Name;
                vnPayModel.OrderId = OrderId; 

                HttpContext.Session.SetString("OrderId", vnPayModel.OrderId.ToString());
                HttpContext.Session.SetString("date", vnPayModel.CreatedDate.ToString());
                HttpContext.Session.SetString("Name", vnPayModel.FullName.ToString());
                HttpContext.Session.SetString("Note", vnPayModel.Description.ToString());
                return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            return View();
        }


        public async Task<ActionResult> PaymentSuccess()
        {
            Payment hd = new Payment();
            hd.IdPayment = hopdongRepository.GenerateHD();
            hd.PayDate = DateTime.Parse(HttpContext.Session.GetString("date"));
            hd.IdUser = HttpContext.Session.GetString("IdKH");
            //hd. = int.Parse(HttpContext.Session.GetString("OrderId"));
            //hd.NoiDung = HttpContext.Session.GetString("Note");
            hd.PayAmount = 69000;
            await hopdongRepository.AddAsync(hd);

            var email = HttpContext.Session.GetString("Email");
            var subject = HttpContext.Session.GetString("OrderId");
            var messsage = HttpContext.Session.GetString("Message");
            await _emailRepository.SendEmailAsync(email, subject, messsage);

            var kh = await _khachHangRepository.GetByIdAsync(hd.IdUser);
            if (kh != null)
            {
                kh.ActivePremium = true;
                await _khachHangRepository.UpdateAsync(kh);
            }
            return View("Success");
        }
        public IActionResult PaymentFail()
        {
            return View();
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }
            
            TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("PaymentSuccess");
        }
    }
}
