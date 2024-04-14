using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectMangaSmurf.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BoTruyenManager : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly ILoaiTruyenRepository _loaiTruyenRepository;
        private readonly ITacGiaRepository _tacGiaRepository;
        public BoTruyenManager( IboTruyenRepository botruyenrepository,
                                IChapterRepository chapterrepository,
                                IKhachHangRepository khachHangRepository,
                                ILoaiTruyenRepository loaiTruyenRepository,
                                ITacGiaRepository tacGiaRepository)
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
            _loaiTruyenRepository = loaiTruyenRepository;
            _tacGiaRepository = tacGiaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var listBotruyen = await _botruyenrepository.GetAllAsync();
            return View(listBotruyen);
        }

        public async Task<IActionResult> Add()
        {
            var Types = await _loaiTruyenRepository.GetAllAsync();
            var Tgs = await _tacGiaRepository.GetAllAsync();
            ViewBag.Types = new SelectList(Types, "IdLoai", "TenLoai");
            ViewBag.TGs = new SelectList(Tgs, "IdTg", "TenTg");

            return View();
        }


        public bool RangBuoc(BoTruyen bt)
        {
            //string input = "123";
            //int number;
            //bool isNumeric = int.TryParse(bt.Dotuoi, out number);
            if (bt.TenBo == null)
            {
                ModelState.AddModelError(string.Empty, "bạn chưa nhập tên truyện");
                return false;
            }
            if (bt.Mota == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp mô tả");
                return false;
            }
            if (bt.Dotuoi == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp Độ tuổi");
                return false;
            }
            if (bt.IdTg == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp tac gia");
                return false;
            }
            if (bt.IdTg == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp trạng thái");
                return false;
            }
            return true;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BoTruyen boTruyen, IFormFile AnhBanner, IFormFile AnhBia)
        {
            var rangbuoc = RangBuoc(boTruyen);
            if (rangbuoc)
            {
                if (AnhBanner != null && AnhBia != null)
                {
                    boTruyen.IdBo = _botruyenrepository.GenerateBoTruyenId();
                    boTruyen.TkDanhgia = 0;
                    boTruyen.TkTheodoi = 0;
                    boTruyen.TongLuotXem = 0;
                    boTruyen.TtPemium = false;
                    boTruyen.AnhBanner = await SaveImage(AnhBanner);
                    boTruyen.AnhBia = await SaveImage(AnhBia);
                    boTruyen.Active = true;
                }
                await _botruyenrepository.AddAsync(boTruyen);

                foreach (var i in boTruyen.listloai)
                {
                    CtLoaiTruyen ct = new CtLoaiTruyen();
                    ct.IdLoai = i;
                    ct.IdBo = boTruyen.IdBo;
                    ct.Active = true;
                    await _loaiTruyenRepository.AddAsyncCTLoai(ct);
                }

                return RedirectToAction("AddChapter","BoTruyenManager", new { id = boTruyen.IdBo.ToString()} );
            }
            else
            {
                var Tgs = await _tacGiaRepository.GetAllAsync();
                ViewBag.TGs = new SelectList(Tgs, "IdTg", "TenTg");
                var Types = await _loaiTruyenRepository.GetAllAsync();
                ViewBag.Types = new SelectList(Types, "IdLoai", "TenLoai");
                return View(boTruyen);
            }
        }

        public bool RangBuocChapter(Chapter bt)
        {
            if (bt.SttChap == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp stt chap");
                return false;
            }
            if (bt.TenChap == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp ten chap");
                return false;
            }
            if (bt.ThoiGian == null)
            {
                ModelState.AddModelError(string.Empty, "Ban Chưa nhâp thoi gian");
                return false;
            }
            return true;
        }

        public async Task<IActionResult> AddChapter(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChapter(Chapter chapter, List<IFormFile> images)
        {
            var rangbuoc = RangBuocChapter(chapter);
            if (rangbuoc)
            {
                await _chapterrepository.AddAsync(chapter);

                int i = 1;
                foreach (var image in images)
                {
                    CtChapter CTChap = new CtChapter();
                    CTChap.SoTrang = i;
                    CTChap.IdBo = chapter.IdBo;
                    CTChap.SttChap = chapter.SttChap;
                    CTChap.AnhTrang = await SaveImageChapter(image);
                    CTChap.Active = true;
                    await _chapterrepository.AddAsyncCT(CTChap);
                    i++;
                }
                TempData["Message"] = "New chapter added successfully, do you want to continue?";
                return RedirectToAction("AddChapter", "BoTruyenManager", new { id = chapter.IdBo.ToString() });
            }
            else
            {
                return View(chapter.IdBo.ToString());
            }
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/truyen", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/truyen/" + image.FileName;
        }
        private async Task<string> SaveImageChapter(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/chapter", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/chapter/" + image.FileName;
        }
    }
}
