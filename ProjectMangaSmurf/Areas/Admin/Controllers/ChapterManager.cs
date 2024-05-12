using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

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

        [HttpPost]
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

            // Ensure the correct route parameters are used
            return RedirectToAction(nameof(Detail), new { id = comicId, stt = chapterNumber });
        }


        [HttpPost]
        public async Task<IActionResult> ToggleActive(string comicId, int chapterNumber)
        {
            // Validate the input parameters.
            if (string.IsNullOrEmpty(comicId) || chapterNumber <= 0)
            {
                TempData["Error"] = "Invalid comic ID or chapter number.";
                return RedirectToAction(nameof(Index));
            }

            // Retrieve the chapter using the provided comic ID and chapter number.
            var chapter = await _chapterrepository.GetByIdAsync(comicId, chapterNumber);
            if (chapter == null)
            {
                TempData["Error"] = "Chapter not found.";
                return NotFound();
            }

            // Toggle the active status of the chapter.
            chapter.Active = !chapter.Active;
            await _chapterrepository.UpdateAsync(chapter);

            // Optionally add a success message or similar feedback.
            TempData["Message"] = $"Chapter active status updated successfully to {(chapter.Active ? "Active" : "Inactive")}.";

            // Redirect back to the detail view of the chapter to see the change.
            return RedirectToAction(nameof(Detail), new { id = comicId, stt = chapterNumber });
        }



        public bool RangBuoc(BoTruyen bt)
        {

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

                foreach (var i in boTruyen.Listloai)
                {
                    CtLoaiTruyen ct = new CtLoaiTruyen();
                    ct.IdLoai = i.ToString();
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
            return currentMaxNumber + 1; // Increment to get the next chapter number
        }

        //  --------------------------------------------------------------------


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

        [HttpPost]
        public async Task<IActionResult> ToggleCTActive(string idBo, int sttChap, int soTrang)
        {
            var ctChapter = await _chapterrepository.GetCTChapterByIdAsync(idBo, sttChap, soTrang);
            if (ctChapter == null)
            {
                TempData["Error"] = "Chapter page not found.";
                return NotFound();
            }

            // Toggle the active status
            ctChapter.Active = !ctChapter.Active;
            await _chapterrepository.UpdateCTChapterAsync(ctChapter);

            TempData["Message"] = $"Page active status updated successfully to {(ctChapter.Active ? "Active" : "Inactive")}.";
            return RedirectToAction("Detail", new { id = idBo, stt = sttChap });
        }


        [HttpPost]
        public async Task<IActionResult> ReplaceImage(string idBo, int sttChap, int soTrang, IFormFile newImage)
        {
            if (newImage != null && newImage.Length > 0)
            {
                await _chapterrepository.ReplaceImageAsync(idBo, sttChap, soTrang, newImage);
            }
            return RedirectToAction("Detail", new { idBo, sttChap });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string idBo, int sttChap)
        {
            if (string.IsNullOrEmpty(idBo) || sttChap <= 0)
            {
                return Json(new { success = false, message = "Invalid chapter information provided." });
            }

            try
            {
                // Retrieve and delete all CtHoatDong entries
                var ctHoatDongs = await _chapterrepository.GetAllCtHoatDongByIdAsync(idBo, sttChap);
                foreach (var ctHoatDong in ctHoatDongs)
                {
                    await _chapterrepository.DeleteCtHoatDongAsync(ctHoatDong);
                }

                // Retrieve and delete all CtChapters
                var ctChapters = await _chapterrepository.GetAllCTByIdAsync(idBo, sttChap);
                foreach (var ctChapter in ctChapters)
                {
                    await _chapterrepository.DeleteCTChapterAsync(ctChapter);
                }

                // Retrieve and delete the chapter
                var chapter = await _chapterrepository.GetByIdAsync(idBo, sttChap);
                if (chapter != null)
                {
                    await _chapterrepository.DeleteChapterAsync(chapter);
                }

                return Json(new { success = true, message = "Chapter and all related activities and pages deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting chapter: " + ex.Message });
            }
        }



    }
}
