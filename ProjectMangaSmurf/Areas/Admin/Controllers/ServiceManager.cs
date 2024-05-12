using Microsoft.AspNetCore.Mvc;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    public class ServiceManager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
