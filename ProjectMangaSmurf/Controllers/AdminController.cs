using Microsoft.AspNetCore.Mvc;

namespace ProjectMangaSmurf.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
