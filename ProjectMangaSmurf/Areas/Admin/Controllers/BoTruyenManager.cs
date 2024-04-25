using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectMangaSmurf.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Macs;

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
        private readonly IAuthorRepository _authRepository;
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

        public async Task<IActionResult> ViewList()
        {
            var listBotruyen = await _botruyenrepository.GetAllAllAsync(); // Correct use of repository
            return View(listBotruyen);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var product = await _botruyenrepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var relatedComics = await _chapterrepository.GetChaptersByComicId(id);
            ViewBag.RelatedChapter = relatedComics ?? new List<ProjectMangaSmurf.Models.Chapter>();
            ViewBag.RelatedSeriesCount = relatedComics.Count;

            return View(product);
        }

        public async Task<IActionResult> ToggleActive(string id)
        {
            var boTruyen = await _botruyenrepository.GetByIdAsync(id);
            if (boTruyen == null)
            {
                return NotFound();
            }

            boTruyen.Active = !boTruyen.Active;
            await _botruyenrepository.UpdateAsync(boTruyen);
            return RedirectToAction(nameof(Detail), new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> TogglePremium(string id)
        {
            var comic = await _botruyenrepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound("Comic not found.");
            }

            // Toggle the premium status of the comic.
            comic.TtPemium = !comic.TtPemium;
            await _botruyenrepository.UpdateAsync(comic);

            // Retrieve all chapters of the comic and update their premium status.
            var chapters = await _chapterrepository.GetChaptersByComicId(id);
            foreach (var chapter in chapters)
            {
                chapter.TtPemium = comic.TtPemium;
                await _chapterrepository.UpdateAsync(chapter);
            }

            TempData["Message"] = "Comic and all related chapters' premium status updated successfully!";
            return RedirectToAction(nameof(Detail), new { id = id });
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
            if (bt.TkDanhgia == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa nhập đánh giá.");
                return false;
            }
            else if (bt.TkDanhgia < 0 || bt.TkDanhgia > 10)
            {
                ModelState.AddModelError(string.Empty, "Đánh giá phải từ 0 đến 10.");
                return false;
            }
            return true;

        }
        [HttpPost]
        public async Task<IActionResult> Add(BoTruyen boTruyen, IFormFile AnhBanner, IFormFile AnhBia)
        {
            var rangbuoc = RangBuoc(boTruyen);
            if (!rangbuoc)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = string.Join(" ", errors) });
            }

            if (AnhBanner != null)
            {
                boTruyen.AnhBanner = await SaveImage(AnhBanner);
            }
            if (AnhBia != null)
            {
                boTruyen.AnhBia = await SaveImage(AnhBia);
            }

            boTruyen.IdBo = _botruyenrepository.GenerateBoTruyenId();
            boTruyen.TkDanhgia = 0;
            boTruyen.TrangThai = 1;
            boTruyen.TkTheodoi = 0;
            boTruyen.TongLuotXem = 0;
            await _botruyenrepository.AddAsync(boTruyen);

            // Assume success if no exceptions thrown
            return Json(new { success = true, message = "Comic added successfully!", redirectUrl = Url.Action("Index") });
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path.Substring(path.IndexOf("wwwroot") + "wwwroot".Length).Replace('\\', '/');
        }

        public async Task<IActionResult> Update(string id)
        {
            var product = await _botruyenrepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var Types = await _loaiTruyenRepository.GetAllAsync();
            var Tgs = await _tacGiaRepository.GetAllAsync();
            ViewBag.Types = new SelectList(Types, "IdLoai", "TenLoai");
            ViewBag.TGs = new SelectList(Tgs, "IdTg", "TenTg");
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(string id, BoTruyen boTruyen, IFormFile AnhBanner, IFormFile AnhBia)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validation errors: " + string.Join(", ", errors) });
            }

            if (id != boTruyen.IdBo)
            {
                return Json(new { success = false, message = "Mismatched ID." });
            }

            var existingProduct = await _botruyenrepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            // Handling Image Upload
            if (AnhBanner != null)
            {
                existingProduct.AnhBanner = await SaveImage(AnhBanner);
            }
            if (AnhBia != null)
            {
                existingProduct.AnhBia = await SaveImage(AnhBia);
            }

            // Update other properties
            existingProduct.TenBo = boTruyen.TenBo;
            existingProduct.Dotuoi = boTruyen.Dotuoi;
            existingProduct.IdTg = boTruyen.IdTg;
            existingProduct.Mota = boTruyen.Mota;
            existingProduct.TtPemium = boTruyen.TtPemium;
            existingProduct.TrangThai = boTruyen.TrangThai;
            existingProduct.Active = boTruyen.Active;
            existingProduct.listloai = boTruyen.listloai; // Make sure this property is correctly bound and updated

            await _botruyenrepository.UpdateAsync(existingProduct);

            return Json(new { success = true, message = "Update successful!" });
        }

        public bool RangBuocChapter(Chapter bt)
        {

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

        public async Task<IActionResult> AddChapter(string id , int stt )
        {
            ViewBag.Id = id;
            ViewBag.Stt = stt;
            var relatedComics = await _chapterrepository.GetChaptersByComicId(id);
            ViewBag.RelatedChapter = relatedComics ?? new List<ProjectMangaSmurf.Models.Chapter>();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> AddChapter(Chapter chapter, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, directly return to the view with the current model to show validation errors.
                return View(chapter);
            }

            var rangbuoc = RangBuocChapter(chapter);
            if (!rangbuoc)
            {
                // If business rules validation fails, return to the view with the current model.
                return View(chapter);
            }

            // Proceed with adding the chapter if all validations pass.
            await _chapterrepository.AddAsync(chapter);

            int i = 1;
            foreach (var image in images)
            {
                CtChapter CTChap = new CtChapter
                {
                    SoTrang = i++,
                    IdBo = chapter.IdBo,
                    SttChap = chapter.SttChap,
                    AnhTrang = await SaveImageChapter(image),
                    Active = true
                };
                await _chapterrepository.AddAsyncCT(CTChap);
            }

            TempData["Message"] = "New chapter added successfully, do you want to continue?";
            return RedirectToAction("AddChapter", new { id = chapter.IdBo });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Lấy bộ truyện cần xóa dựa trên ID
            var boTruyen = await _botruyenrepository.GetByIdAsync(id);
            if (boTruyen == null)
            {
                return NotFound();
            }
            await _botruyenrepository.DeleteAllChaptersAndDetails(id);

            await _botruyenrepository.DeleteAsync(id);

            return RedirectToAction(nameof(ViewList));
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
