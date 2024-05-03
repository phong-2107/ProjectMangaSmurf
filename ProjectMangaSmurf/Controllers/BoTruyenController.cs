using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
namespace ProjectMangaSmurf.Controllers
{
    public class BoTruyenController : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly ICTBoTruyenRepository _cTBoTruyenRepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        public BoTruyenController(IboTruyenRepository botruyenrepository, IChapterRepository chapterrepository, IKhachHangRepository khachHangRepository, ICTBoTruyenRepository cTBoTruyenRepository)
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
            _cTBoTruyenRepository = cTBoTruyenRepository;
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

        [HttpGet]
        public async Task<IActionResult> SearchBoTruyen(string query)
        {
            var results = await _botruyenrepository.SearchByNameAsync(query);
            return Json(results.Select(x => new { id = x.IdBo , img = x.AnhBia, tenBo = x.TenBo, view = x.TongLuotXem}));
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
            if (Botruyen == null)
            {
                return NotFound();
            }
            var kh = HttpContext.Session.GetString("TK");
            if (kh != null)
            {
                var khtmp = await _khachhangrepository.GetByAccountAsync(kh);
                var findct = _cTBoTruyenRepository.GetByIdFollowAsync(khtmp.IdUser, id);
                if (await findct != null)
                {
                    var ls = await findct;
                    ViewBag.follow = true;
                    ViewBag.next = ls.LsMoi;
                }
                else
                {
                    ViewBag.follow = false;
                }
               
            }
            else
            {
                ViewBag.follow = false;
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
            var kh = HttpContext.Session.GetString("TK");
            if (kh != null)
            {
                var khtmp = await _khachhangrepository.GetByAccountAsync(kh);
                CtHoatDong ct = new CtHoatDong();
                ct.IdBo = id;
                ct.SttChap = stt;
                ct.IdUser = khtmp.IdUser;
                ct.TtDoc = true;
                await _chapterrepository.AddAsyncCTHD(ct);

                var ctBo = _cTBoTruyenRepository.GetByIdAsync(khtmp.IdUser, id);
                if(await ctBo != null)
                {
                    var bo = await ctBo;
                    bo.LsMoi = stt.ToString();
                    await _cTBoTruyenRepository.UpdateAsync(bo);
                }
                else
                {
                    CtBoTruyen ctBoTruyen = new CtBoTruyen();
                    ctBoTruyen.IdUser = khtmp.IdUser;
                    ctBoTruyen.IbBo = id;
                    ctBoTruyen.Theodoi = false;
                    ctBoTruyen.DanhGia = 0;
                    ctBoTruyen.LsMoi = stt.ToString();
                    await _cTBoTruyenRepository.AddAsync(ctBoTruyen);
                    ViewBag.follow = false;
                }

                var findct = _cTBoTruyenRepository.GetByIdAsync(khtmp.IdUser, id);
                if (await findct != null)
                {
                    var follow  = await findct;
                    if(follow.Theodoi == false)
                        ViewBag.follow = false;
                    else
                        ViewBag.follow = true;
                }
                else
                {
                    ViewBag.follow = false;
                }
            }
            else
            {
                ViewBag.follow = false;
            }
            return View(Chapter);
        }


    }
}
