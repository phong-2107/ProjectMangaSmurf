using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using ProjectMangaSmurf.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;
namespace ProjectMangaSmurf.Controllers
{
    public class BoTruyenController : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly ICTBoTruyenRepository _cTBoTruyenRepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly IHopdongRepository _hopdongRepository;
        private readonly IBookRecommendationRepository _recommendationRepository;
        private readonly ProjectDBContext _context;


        public BoTruyenController(  ProjectDBContext db,
                                    IboTruyenRepository botruyenrepository, 
                                    IChapterRepository chapterrepository, 
                                    IKhachHangRepository khachHangRepository, 
                                    ICTBoTruyenRepository cTBoTruyenRepository, 
                                    IHopdongRepository hopdongRepository,
                                    IHttpClientFactory clientFactory, 
                                    IBookRecommendationRepository recommendationRepository)
        {
            _context = db;
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
            _cTBoTruyenRepository = cTBoTruyenRepository;
            _hopdongRepository = hopdongRepository;
            _recommendationRepository = recommendationRepository;
        }

        public async Task<IActionResult> Index()
        {
            var listBotruyen = await _botruyenrepository.GetAllAsync();
            var listLoaiTruyen = await _botruyenrepository.GetAllLoaiTruyens();
            ViewBag.LoaiTruyen = listLoaiTruyen;

            var userId = HttpContext.Session.GetString("IdKH");
            if (userId != null)
            {
                ViewBag.kh = await _khachhangrepository.GetByIdAsync(userId);
                ViewBag.pay = await _hopdongRepository.GetPaymentByIdAsync(userId);
            }
            else
            {
                ViewBag.kh = null;
            }

            return View(listBotruyen);
        }
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
            return Json(results.Select(x => new { id = x.IdBo, img = x.AnhBia, tenBo = x.TenBo, view = x.TongLuotXem }));
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
            if (id == 3)
            {
                var listRate = await _botruyenrepository.GetAllAsyncByRate();
                return View(listRate);
            }
            if (id == 4)
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

        public static int ExtractNumberFromId(string id)
        {
            var match = Regex.Match(id, "^BT([0-9]+)$");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            throw new ArgumentException("ID không hợp lệ");
        }

        private string CreateIdFromNumber(int number)
        {
            return "BT" + number.ToString("D8");
        }

        public async Task<IActionResult> CTBoTruyen(string id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Botruyen = await _botruyenrepository.GetByIdAsync(id);
                    if (Botruyen == null)
                    {
                        return NotFound();
                    }

                    var kh = HttpContext.Session.GetString("IdKH");
                    if (kh != null)
                    {
                        var khtmp = await _khachhangrepository.GetByIdAsync(kh);
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
                        var bookId = ExtractNumberFromId(id);
                        var recommendations = await _recommendationRepository.GetRecommendationsAsync(bookId);

                        List<BoTruyen> list = new List<BoTruyen>();

                        foreach (var r in recommendations)
                        {
                            var idBo = CreateIdFromNumber(r);
                            BoTruyen bt = await _botruyenrepository.GetByIdAsync(idBo);
                            if (bt != null)
                            {
                                list.Add(bt);
                            }
                        }

                        var rating = await _cTBoTruyenRepository.GetByIdAsync(kh, id);
                        if(rating!= null)
                        {
                            ViewBag.rating = rating.DanhGia;
                        }
                        else
                        {
                            ViewBag.rating = "0";
                        }

                        ViewBag.list = list;
                        ViewBag.premium = khtmp.ActivePremium;
                        if (khtmp.TicketSalary != 0)
                        {
                            ViewBag.cost = khtmp.TicketSalary;
                        }


                    }
                    else
                    {
                        ViewBag.follow = false;
                        ViewBag.rating = "0";
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit(); // Commit transaction if all operations are successful

                    return View(Botruyen);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return View("Error"); 
                }
            }
        }
        public async Task<IActionResult> Chapter(string id, int stt)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var chapter = await _chapterrepository.GetByIdAsync(id, stt);
                    if (chapter == null)
                    {
                        return NotFound();
                    }
                    chapter.TkLuotxem += 1;
                    await _chapterrepository.UpdateAsync(chapter);

                    await UpdateBoTruyenViews(id);

                    var userId = HttpContext.Session.GetString("IdKH");
                    if (userId != null)
                    {

                        await HandleUserActivity(userId, id, stt);
                    }
                    else
                    {
                        ViewBag.follow = false;
                    }

                    await transaction.CommitAsync();
                    return View(chapter);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Internal server error");
                }
            }
        }

        private async Task UpdateBoTruyenViews(string id)
        {
            var bt = await _botruyenrepository.GetByIdAsync(id);
            if (bt != null)
            {
                bt.TongLuotXem += 1;
                await _botruyenrepository.UpdateAsync(bt);
            }
        }

        private async Task HandleUserActivity(string userId, string boId, int stt)
        {
            var user = await _khachhangrepository.GetByIdAsync(userId);
            if (user == null) return;

            var ctHoatDong = new CtHoatDong
            {
                IdBo = boId,
                SttChap = stt,
                IdUser = user.IdUser,
                TtDoc = true
            };
            await _chapterrepository.AddAsyncCTHD(ctHoatDong);

            var ctBo = await _cTBoTruyenRepository.GetByIdAsync(user.IdUser, boId);
            if (ctBo != null)
            {
                ctBo.LsMoi = stt.ToString();
                await _cTBoTruyenRepository.UpdateAsync(ctBo);
            }
            else
            {
                var ctBoTruyen = new CtBoTruyen
                {
                    IdUser = user.IdUser,
                    IbBo = boId,
                    Theodoi = false,
                    DanhGia = 0,
                    LsMoi = stt.ToString()
                };
                await _cTBoTruyenRepository.AddAsync(ctBoTruyen);
            }

            var findChap = await _chapterrepository.GetByIdAsync(boId, stt);
            if (findChap != null)
            {
                user.TicketSalary = user.TicketSalary - findChap.TicketCost;
                await _khachhangrepository.UpdateAsync(user);
            }

            ViewBag.follow = ctBo?.Theodoi ?? false;
        }
    }
}