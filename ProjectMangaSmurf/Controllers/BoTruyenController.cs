using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Rankings(int id)
        {
            ViewBag.Value = id;
            if (id == 1)
            {
                var listView = await _botruyenrepository.GetRankingAsync();
                return View(listView);
            }
            if (id == 2)
            {
                var listFollow = await _botruyenrepository.GetAllAsyncByFollow();
                return View(listFollow);
            }
            if (id == 3){
                var listRate = await _botruyenrepository.GetAllAsyncByRate();
                return View(listRate);
            }
            if(id == 4)
            {
                var listToday = await _botruyenrepository.GetAllAsyncByDay();
                return View(listToday);
            }
            if (id == 5)
            {
                var listMonth = await _botruyenrepository.GetAllAsyncByMonth();
                return View(listMonth);
            }
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
            else
            {
                Chapter.TkLuotxem = Chapter.TkLuotxem + 1;
                await _chapterrepository.UpdateAsync(Chapter);

                var bt = await _botruyenrepository.GetByIdAsync(id);
                if(bt != null)
                {
                    bt.TongLuotXem = bt.TongLuotXem + 1;
                    await _botruyenrepository.UpdateAsync(bt);
                }
            }
            var kh = HttpContext.Session.GetObjectFromJson<KhachHang>("kh");
            if(kh != null)
            {
                ViewBag.user = kh;
            }
            else
            {
                ViewBag.user = null;
            }
            return View(Chapter);
        }
        //[HttpPost]
        //public async Task<IActionResult> OnChapter([FromBody] string id)
        //{
        //    // Lấy Chapter từ id
        //    var chapter = await _chapterrepository.GetByIdAsync(id);

        //    if (chapter != null)
        //    {
        //        // Tăng số lượt xem
        //        chapter.TkLuotxem += 1;

        //        // Cập nhật Chapter
        //        await _chapterrepository.UpdateAsync(chapter);

        //        // Lấy thông tin bộ truyện tương ứng
        //        var bt = await _botruyenrepository.GetByIdAsync(id);
        //        if (bt != null)
        //        {
        //            // Tăng tổng số lượt xem
        //            bt.TongLuotXem += 1;

        //            // Cập nhật thông tin bộ truyện
        //            await _botruyenrepository.UpdateAsync(bt);
        //        }
        //    }

        //    return Ok(); // Hoặc bạn có thể trả về bất kỳ dữ liệu nào bạn muốn trả về
        //}

    }
}
