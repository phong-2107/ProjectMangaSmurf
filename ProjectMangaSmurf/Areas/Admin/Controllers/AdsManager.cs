using Microsoft.AspNetCore.Mvc;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    public class AdsManager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
