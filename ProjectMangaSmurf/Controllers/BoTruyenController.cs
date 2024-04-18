using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using Microsoft.AspNetCore.Mvc;
namespace ProjectMangaSmurf.Controllers
{
    public class BoTruyenController : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        public BoTruyenController(IboTruyenRepository botruyenrepository, IChapterRepository chapterrepository, IKhachHangRepository khachHangRepository)
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
        }
        public async Task<IActionResult> Index()
        {
            var listBotruyen = await _botruyenrepository.GetAllAsync();
            var listLoaiTruyen = await _botruyenrepository.GetAllLoaiTruyens();
            ViewBag.LoaiTruyen = listLoaiTruyen;
            return View(listBotruyen);
        }


        //public async Task<IActionResult> Index(string id)
        //{
        //    var listBotruyen = await _khachhangrepository.GetByIdAsync(id);
        //    return View(listBotruyen);
        //}

        public async Task<IActionResult> ListTruyen()
        {
            var listBotruyen = await _botruyenrepository.GetAllAsync();
            return View(listBotruyen);
        }

        public async Task<IActionResult> ListTruyenEarliest()
        {
            var listBotruyen = await _botruyenrepository.GetAllAsyncByChapterEarliest();
            return View(listBotruyen);
        }

        public async Task<IActionResult> ListTruyenTT(int id)
        {
            var listBotruyen = await _botruyenrepository.GetAllTrangThaiAsync(id);
            return View(listBotruyen);
        }

        public async Task<IActionResult> Rankings()
        {

            var listBotruyen = await _botruyenrepository.GetRankingAsync();
            return View(listBotruyen);
        }
        public async Task<IActionResult> Trending()
        {
            var listBotruyen = await _botruyenrepository.GetTrendingAsync();
            return View(listBotruyen);
        }

        public async Task<IActionResult> ListTopic(string id)
        {
            var listBotruyen = await _botruyenrepository.GetAllByTopic(id);
            var loai = await _botruyenrepository.GetLoaiByNameAsync(id);
            ViewBag.Topic = loai.TenLoai;
            return View(listBotruyen);
        }

        

        public async Task<IActionResult> CTBoTruyen(string id)
        {
            var Botruyen = await _botruyenrepository.GetByIdAsync(id);
            var Chapter = await _chapterrepository.GetAllAsync();
            if (Botruyen == null)
            {
                return NotFound();
            }

            return View(Botruyen);
        }

        public async Task<IActionResult> Chapter(string id, int stt)
        {
            var Chapter = await _chapterrepository.GetByIdAsync(id, stt);
            if (Chapter == null)
            {
                return NotFound();
            }
            return View(Chapter);
        }
    }
}
