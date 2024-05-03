using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailRepository;
        public ContactController(IEmailService emailRepository)
        {
            _emailRepository = emailRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string email, string subject, string message)
        {
            if (ModelState.IsValid)
            {
                await _emailRepository.SendEmailAsync(email, subject, message);
                return RedirectToAction("Success", "Payment");
            }
			return RedirectToAction("Index");
		}
    }
}
