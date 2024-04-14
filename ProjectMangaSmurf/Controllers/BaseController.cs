using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            KhachHang? khachHang = HttpContext.Session.GetObjectFromJson<KhachHang>("kh") ?? new KhachHang();
            ViewBag.KhachHang = khachHang;
        }
    }
}
