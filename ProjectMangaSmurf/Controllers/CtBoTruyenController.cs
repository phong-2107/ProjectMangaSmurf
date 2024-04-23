using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Controllers
{
    public class CtBoTruyenController : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly ICTBoTruyenRepository _cTBoTruyenRepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        public CtBoTruyenController(IboTruyenRepository botruyenrepository, IChapterRepository chapterrepository, IKhachHangRepository khachHangRepository, ICTBoTruyenRepository cTBoTruyenRepository)
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
            _cTBoTruyenRepository = cTBoTruyenRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
