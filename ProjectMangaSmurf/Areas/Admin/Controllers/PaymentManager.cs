using Microsoft.AspNetCore.Mvc;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    public class PaymentManager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
