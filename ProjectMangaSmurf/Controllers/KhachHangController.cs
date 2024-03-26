using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
namespace ProjectMangaSmurf.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        public KhachHangController(IboTruyenRepository botruyenrepository, IChapterRepository chapterrepository)
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
        }



        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }


    }
}
