using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class ChapterManager : Controller
    {
        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly ILoaiTruyenRepository _loaiTruyenRepository;
        private readonly ITacGiaRepository _tacGiaRepository;
        private readonly ProjectDBContext _context;

        public ChapterManager(IboTruyenRepository botruyenrepository,
                                IChapterRepository chapterrepository,
                                IKhachHangRepository khachHangRepository,
                                ILoaiTruyenRepository loaiTruyenRepository,
                                ITacGiaRepository tacGiaRepository,
                                ProjectDBContext context)
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
            _loaiTruyenRepository = loaiTruyenRepository;
            _tacGiaRepository = tacGiaRepository;
            _context = context;
        }

        #region Index
        [RBACAuthorize(PermissionId = 20)]
        public async Task<IActionResult> Index()
        {
            var listBotruyen = await _botruyenrepository.GetAllAsync();
            return View(listBotruyen);
        }
        #endregion

        #region Add
        [RBACAuthorize(PermissionId = 22)]
        public IActionResult Add()
        {
            return View();
        }
        #endregion

        #region Detail
        [RBACAuthorize(PermissionId = 21)]
        public async Task<IActionResult> Detail(string id, int stt)
        {
            if (string.IsNullOrEmpty(id) || stt <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var chapter = await _chapterrepository.GetByIdAsync(id, stt);
            if (chapter == null)
            {
                return NotFound();
            }

            var cmmb = await _chapterrepository.GetAllCTByIdAsync(id, stt);
            var lmao = await _botruyenrepository.GetAllAllAsync();
            ViewBag.RelatedPage = cmmb ?? new List<CtChapter>();
            ViewBag.RelatedPag = lmao ?? new List<BoTruyen>();
            return View(chapter);
        }
        #endregion

        #region TogglePremium
        [HttpPost]
        [RBACAuthorize(PermissionId = 25)]
        public async Task<IActionResult> TogglePremium(string comicId, int chapterNumber)
        {
            var chapter = await _chapterrepository.GetByIdAsync(comicId, chapterNumber);
            if (chapter == null)
            {
                return NotFound();
            }

            chapter.TtPemium = !chapter.TtPemium;
            await _chapterrepository.UpdateAsync(chapter);

            var chapters = await _chapterrepository.GetChaptersByComicId(comicId);
            bool anyPremiumChapters = chapters.Any(c => c.TtPemium);

            var comic = await _botruyenrepository.GetByIdAsync(comicId);
            if (comic != null)
            {
                comic.TtPemium = anyPremiumChapters;
                await _botruyenrepository.UpdateAsync(comic);
            }

            TempData["Message"] = "Chapter and related comic's premium status updated successfully!";
            return RedirectToAction(nameof(Detail), new { id = comicId, stt = chapterNumber });
        }
        #endregion

        #region ToggleActive
        [HttpPost]
        public async Task<IActionResult> ToggleActive(string comicId, int chapterNumber)
        {
            if (string.IsNullOrEmpty(comicId) || chapterNumber <= 0)
            {
                TempData["Error"] = "Invalid comic ID or chapter number.";
                return RedirectToAction(nameof(Index));
            }

            var chapter = await _chapterrepository.GetByIdAsync(comicId, chapterNumber);
            if (chapter == null)
            {
                TempData["Error"] = "Chapter not found.";
                return NotFound();
            }

            chapter.Active = !chapter.Active;
            await _chapterrepository.UpdateAsync(chapter);

            TempData["Message"] = $"Chapter active status updated successfully to {(chapter.Active ? "Active" : "Inactive")}.";
            return RedirectToAction(nameof(Detail), new { id = comicId, stt = chapterNumber });
        }
        #endregion

        #region AddChapter

        private bool RangBuoc(BoTruyen bt)
        {
            if (bt.TenBo == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa nhập tên truyện");
                return false;
            }
            if (bt.Mota == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa nhập mô tả");
                return false;
            }
            if (bt.Dotuoi == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa nhập độ tuổi");
                return false;
            }
            if (bt.IdTg == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa nhập tác giả");
                return false;
            }
            if (bt.IdTg == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn chưa nhập trạng thái");
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

                foreach (var i in boTruyen.Listloai)
                {
                    CtLoaiTruyen ct = new CtLoaiTruyen
                    {
                        IdLoai = i.ToString(),
                        IdBo = boTruyen.IdBo,
                        Active = true
                    };
                    await _loaiTruyenRepository.AddAsyncCTLoai(ct);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var Types = await _loaiTruyenRepository.GetAllAsync();
                ViewBag.Types = new SelectList(Types, "IdLoai", "TenLoai");
                return View(boTruyen);
            }
        }
        #endregion

        #region UpdatePageNumber
        public async Task<IActionResult> UpdatePageNumberForm(string idBo, int sttChap, int soTrang)
        {
            ViewBag.CurrentPageNumber = soTrang;
            ViewBag.MaxPageNumber = await _chapterrepository.GetMaxPageNumber(idBo, sttChap);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePageNumber(string idBo, int sttChap, int currentSoTrang, int newSoTrang)
        {
            try
            {
                await _chapterrepository.UpdatePageNumberAsync(idBo, sttChap, currentSoTrang, newSoTrang);
                return RedirectToAction("Detail", new { id = idBo, stt = sttChap });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }
        }
        #endregion

        #region ToggleCTActive
        [HttpPost]
        [RBACAuthorize(PermissionId = 25)]
        public async Task<IActionResult> ToggleCTActive(string idBo, int sttChap, int soTrang)
        {
            var ctChapter = await _chapterrepository.GetCTChapterByIdAsync(idBo, sttChap, soTrang);
            if (ctChapter == null)
            {
                TempData["Error"] = "Chapter page not found.";
                return NotFound();
            }

            ctChapter.Active = !ctChapter.Active;
            await _chapterrepository.UpdateCTChapterAsync(ctChapter);

            TempData["Message"] = $"Page active status updated successfully to {(ctChapter.Active ? "Active" : "Inactive")}.";
            return RedirectToAction("Detail", new { id = idBo, stt = sttChap });
        }
        #endregion

        #region ReplaceImage
        [HttpPost]
        public async Task<IActionResult> ReplaceImage(string idBo, int sttChap, int soTrang, IFormFile newImage)
        {
            if (newImage != null && newImage.Length > 0)
            {
                await _chapterrepository.ReplaceImageAsync(idBo, sttChap, soTrang, newImage);
            }
            return RedirectToAction("Detail", new { idBo, sttChap });
        }
        #endregion

        #region DeleteConfirmed
        [HttpPost]
        [RBACAuthorize(PermissionId = 24)]
        public async Task<IActionResult> DeleteConfirmed(string idBo, int sttChap)
        {
            if (string.IsNullOrEmpty(idBo) || sttChap <= 0)
            {
                return Json(new { success = false, message = "Invalid chapter information provided." });
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var ctHoatDongs = await _chapterrepository.GetAllCtHoatDongByIdAsync(idBo, sttChap);
                    foreach (var ctHoatDong in ctHoatDongs)
                    {
                        await _chapterrepository.DeleteCtHoatDongAsync(ctHoatDong);
                    }

                    var ctChapters = await _chapterrepository.GetAllCTByIdAsync(idBo, sttChap);
                    foreach (var ctChapter in ctChapters)
                    {
                        await _chapterrepository.DeleteCTChapterAsync(ctChapter);
                    }

                    var chapter = await _chapterrepository.GetByIdAsync(idBo, sttChap);
                    if (chapter != null)
                    {
                        await _chapterrepository.DeleteChapterAsync(chapter);
                    }

                    await transaction.CommitAsync();
                    return Json(new { success = true, message = "Chapter and all related activities and pages deleted successfully." });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "Error deleting chapter: " + ex.Message });
                }
            }
        }
        #endregion

        #region HelperMethods
        public async Task<IEnumerable<CtChapter>> GetAllCTByIdAsync(string id, int stt)
        {
            return await _chapterrepository.GetAllCTByIdAsync(id, stt);
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

        public async Task<int> GenerateNextChapterNumber(string idBo)
        {
            int currentMaxNumber = await _chapterrepository.GetMaxChapterNumberAsync(idBo);
            return currentMaxNumber + 1;
        }
        #endregion
    }
}
