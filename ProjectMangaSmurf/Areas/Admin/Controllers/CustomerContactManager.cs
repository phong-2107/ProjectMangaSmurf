using Microsoft.AspNetCore.Mvc;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    public class CustomerContactManager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
