using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PayPalCheckoutSdk.Orders;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using System.Diagnostics;


namespace ProjectMangaSmurf.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVNPayRepository _vnPayservice;
        private readonly IHopdongRepository hopdongRepository;
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly IEmailService _emailRepository;
        private readonly ProjectDBContext _context;
        private readonly IPayPalService _payPalService;
        public PaymentController(IKhachHangRepository khachHangRepository, IVNPayRepository vnPayservice,IPayPalService payPalService,  IHopdongRepository hopdong, IEmailService emailRepository, ProjectDBContext context)
        {
            _khachHangRepository = khachHangRepository;
            hopdongRepository = hopdong;
            _vnPayservice = vnPayservice;
            _emailRepository = emailRepository;
            _payPalService = payPalService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout(string id)
        {
            var pack =  hopdongRepository.GetPackById(id);
            HttpContext.Session.SetString("pack", id);
            return View(pack);
        }


        [HttpPost]
        public IActionResult Checkout(double Money, string Name, string phone, int OrderId, string PaymentMethod)
        {

            if (ModelState.IsValid)
            {
                using var transaction = _context.Database.BeginTransaction();  // Giả sử '_context' là DbContext của bạn
                try
                {
                    if (PaymentMethod == "Thanh toán VNPay")
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
                        HttpContext.Session.SetString("Phone", phone);
                        return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
                    }
                    else if(PaymentMethod == "Thanh toán Paypal")
                    {
                        return RedirectToAction("ProcessPayPalPayment", new { Money, Name, phone, OrderId });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return RedirectToAction("Error", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayPalPayment(double Money, string Name, string Phone, int OrderId)
        {
            try
            {
                var order = await _payPalService.CreatePayPalOrder(Money, Name, Phone, OrderId);
                return Redirect(order.Links.FirstOrDefault(l => l.Rel.Equals("approve")).Href);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Unable to process payment with PayPal: " + e.Message;
                return View("Checkout");
            }
        }

        public string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
            }
            return template;
        }


        public async Task<ActionResult> PaymentSuccess()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var tmp = hopdongRepository.GetPackById(HttpContext.Session.GetString("pack"));
                    var pack = tmp as ServicePackConfig;

                    Payment hd = new Payment
                    {
                        IdPayment = hopdongRepository.GenerateHD(),
                        PayDate = DateTime.Parse(HttpContext.Session.GetString("date")),
                        IdUser = HttpContext.Session.GetString("IdKH"),
                        IdPack = pack.IdPack,
                        PayAmount = pack.Amount,
                        PayMethod = 1,
                        ExpiresTime = DateTime.Today.AddMonths(2)
                    };

                    await hopdongRepository.AddAsync(hd);

                    var email = HttpContext.Session.GetString("Email");
                    var subject = "Thanh toán hóa đơn " + HttpContext.Session.GetString("Message");
                    var message = HttpContext.Session.GetString("OrderId");

                    string templatePath = "SendMail.html";
                    string emailTemplate = await _emailRepository.ReadTemplateAsync(templatePath);

                    var date = HttpContext.Session.GetString("date");

                    var placeholders = new Dictionary<string, string>
                    {
                        { "orderId", hd.IdPayment },
                        { "orderValue", hd.PayAmount.ToString() },
                        { "cus", HttpContext.Session.GetString("TK") },
                        { "date", date},
                        { "phone", HttpContext.Session.GetString("Phone") },
                    };

                    string emailBody = ReplacePlaceholders(emailTemplate, placeholders);

                    await _emailRepository.SendEmailTemplateAsync(email, subject, message, emailBody);

                    var kh = await _khachHangRepository.GetByIdAsync(hd.IdUser);
                    if (kh != null)
                    {
                        kh.ActivePremium = true;
                        kh.TicketSalary = kh.TicketSalary + pack.TicketSalary;
                        await _khachHangRepository.UpdateAsync(kh);
                    }
                    transaction.Commit();

                    return View("Success");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return RedirectToAction("Error", "Home");
                }
            }
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

        [HttpPost]
        public async Task<IActionResult> CreatePayment(decimal amount)
        {
            var approvalUrl = await _payPalService.CreateOrderAsync(amount, "USD");
            return Redirect(approvalUrl);
        }

        [HttpGet]
        public async Task<IActionResult> ExecutePayment(string orderId)
        {
            var order = await _payPalService.CaptureOrderAsync(orderId);
            if (order.Status == "COMPLETED")
            {
                return View("Success");
            }
            return View("PaymentFail");
        }
    }
}
