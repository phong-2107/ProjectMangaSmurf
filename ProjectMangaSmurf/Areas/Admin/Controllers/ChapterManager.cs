using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ChapterManager : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly ILoaiTruyenRepository _loaiTruyenRepository;
        private readonly ITacGiaRepository _tacGiaRepository;
        public ChapterManager(IboTruyenRepository botruyenrepository,
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

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
                var Types = await _loaiTruyenRepository.GetAllAsync();
                ViewBag.Types = new SelectList(Types, "IdLoai", "TenLoai");
                return View(boTruyen);
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
    }
}
