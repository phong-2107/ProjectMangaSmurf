using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailRepository _emailRepository;
        public ContactController(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ContactMail model)
        {
            if (ModelState.IsValid)
            {
                await _emailRepository.SendEmailAsync(model);
                return RedirectToAction("Success");
            }
            return View("Index", model);
        }
    }
}
