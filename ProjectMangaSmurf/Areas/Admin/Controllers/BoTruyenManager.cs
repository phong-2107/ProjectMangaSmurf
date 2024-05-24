using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectMangaSmurf.Models;
using Microsoft.EntityFrameworkCore;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;
using ProjectMangaSmurf.Data;
//using ChapterModel = ProjectMangaSmurf.Models.Chapter;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class BoTruyenManager : Controller
    {
        #region Init


        private readonly IboTruyenRepository _botruyenrepository;
        private readonly IChapterRepository _chapterrepository;
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly ILoaiTruyenRepository _loaiTruyenRepository;
        private readonly ITacGiaRepository _tacGiaRepository;
        private readonly IAuthorRepository _authRepository;
        private readonly IHopdongRepository _hopdongRepository;
        private readonly ProjectDBContext _context;  // Thêm dòng này

        public BoTruyenManager(IboTruyenRepository botruyenrepository,
                                IChapterRepository chapterrepository,
                                IKhachHangRepository khachHangRepository,
                                ILoaiTruyenRepository loaiTruyenRepository,
                                ITacGiaRepository tacGiaRepository,
                                IHopdongRepository hopdongRepository,
                                ProjectDBContext context)  // Thêm dòng này
        {
            _botruyenrepository = botruyenrepository;
            _chapterrepository = chapterrepository;
            _khachhangrepository = khachHangRepository;
            _loaiTruyenRepository = loaiTruyenRepository;
            _tacGiaRepository = tacGiaRepository;
            _hopdongRepository = hopdongRepository;
            _context = context;  // Thêm dòng này
        }

        #endregion

        #region Helper Methods
        public decimal SumMoney(IEnumerable<Payment> list)
        {
            decimal money = new decimal();
            if (list != null)
            {
                foreach (var i in list)
                {
                    money += (decimal)i.PayAmount;
                }
                return money;
            }
            else
                return 0;
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

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/truyen", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/truyen/" + image.FileName;
        }

        //private async Task<string> CreatePDFAndSaveImages(string idBo, int sttChap, List<IFormFile> images)
        //{
        //    string directoryPath = Path.Combine("wwwroot", "pdf");
        //    Directory.CreateDirectory(directoryPath);  // Ensure the directory exists

        //    string pdfFileName = $"{idBo}_{sttChap}.pdf";
        //    string pdfPath = Path.Combine(directoryPath, pdfFileName);

        //    using (Document document = new Document(PageSize.A4))
        //    using (FileStream stream = new FileStream(pdfPath, FileMode.Create))
        //    {
        //        PdfWriter.GetInstance(document, stream);
        //        document.Open();

        //        foreach (var image in images)
        //        {
        //            var imagePath = await SaveImage(image, directoryPath);  // Save image temporarily if needed
        //            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imagePath);
        //            pdfImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
        //            pdfImage.Alignment = Image.ALIGN_CENTER | Image.ALIGN_MIDDLE;
        //            document.NewPage();
        //            document.Add(pdfImage);
        //        }

        //        document.Close();
        //    }

        //    return pdfPath.Substring(pdfPath.IndexOf("wwwroot") + "wwwroot".Length).Replace('\\', '/');
        //}

        private async Task<string> SaveImage(IFormFile image, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + Path.GetExtension(image.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return filePath;
        }
        #endregion


        #region Index Methods
        [RBACAuthorize(PermissionId = 19)]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return RedirectToAction("Login", "AdminLogin", new { area = "Admin" });
            }
            else
            {
                var listBotruyen = await _botruyenrepository.GetAllAsync();
                var ListEarliest = await _botruyenrepository.GetAllAsyncByChapterEarliest();
                var listPopular = await _botruyenrepository.GetAllAsyncByDay();
                var listMoney = await _hopdongRepository.GetAllAsync();
                var sumView = await _botruyenrepository.GetAllView();
                var Cus = await _khachhangrepository.GetAllAsync();
                var Money = SumMoney(listMoney);
                ViewBag.Money = Money;
                ViewBag.cus = Cus.Count();
                ViewBag.Bo = listBotruyen.Count();
                ViewBag.View = sumView;
                ViewBag.ListEarliest = ListEarliest.Take(4);
                ViewBag.listPopular = listPopular.Take(4);
                return View(listBotruyen);
            }
        }

        [RBACAuthorize(PermissionId = 13)]
        public async Task<IActionResult> ViewList()
        {
            var listBotruyen = await _botruyenrepository.GetAllAllAsync(); // Correct use of repository
            return View(listBotruyen);
        }

        [RBACAuthorize(PermissionId = 14)]
        public async Task<IActionResult> Detail(string id)
        {
            var product = await _botruyenrepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var relatedComics = await _chapterrepository.GetChaptersByComicId(id);
            ViewBag.RelatedChapter = relatedComics;
            ViewBag.RelatedSeriesCount = relatedComics.Count;

            return View(product);
        }
        #endregion

        #region Add Methods
        [RBACAuthorize(PermissionId = 15)]
        public async Task<IActionResult> Add()
        {
            var Types = await _loaiTruyenRepository.GetAllAsync();
            var Tgs = await _tacGiaRepository.GetAllAsync();
            ViewBag.Types = new SelectList(Types, "IdLoai", "TenLoai");
            ViewBag.TGs = new SelectList(Tgs, "IdTg", "TenTg");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(BoTruyen boTruyen, IFormFile AnhBanner, IFormFile AnhBia)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
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

                    await transaction.CommitAsync();

                    return Json(new { success = true, message = "Comic added successfully!", redirectUrl = Url.Action("ViewList") });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
                }
            }
        }
        public async Task<IActionResult> AddChapterPDF(string id, int stt)
        {
            ViewBag.Id = id;
            ViewBag.Stt = stt;
            var relatedComics = await _chapterrepository.GetChaptersByComicId(id);
            ViewBag.RelatedChapter = relatedComics ?? new List<ProjectMangaSmurf.Models.Chapter>();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> AddChapterPDF(Chapter chapter, List<IFormFile> images)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(chapter);
                    }

                    if (images.Any())
                    {
                        //string pdfPath = await CreatePDFAndSaveImages(chapter.IdBo, chapter.SttChap, images);
                        //chapter.ChapterContent = pdfPath;
                    }

                    await _chapterrepository.AddAsync(chapter);
                    await transaction.CommitAsync();

                    TempData["Message"] = "New chapter added successfully with PDF!";
                    return RedirectToAction("Detail", new { id = chapter.IdBo });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
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
        private async Task<string> SaveImageChapter(IFormFile image)
        {
            var identifier = $"{new Random().Next(1000, 9999)}";
            string fileName = $"{identifier}_{image.FileName}";
            var savePath = Path.Combine("wwwroot/images/chapter", fileName);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/chapter/" + fileName;
        }
        [HttpGet]
        public async Task<IActionResult> AddChapter(string id, int stt)
        {
            var max = await _chapterrepository.GetMaxSttChapAsync(id);
            ViewBag.Id = id;
            ViewBag.Stt = max + 1;
            var relatedComics = await _chapterrepository.GetChaptersByComicId(id);
            ViewBag.RelatedChapter = relatedComics ?? new List<Chapter>();
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddChapter(Chapter chapter, List<IFormFile> images)
        {
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

            var stt = chapter.SttChap;
            TempData["Message"] = "New chapter added successfully, do you want to continue?";
            return RedirectToAction("AddChapter", new { id = chapter.IdBo,  stt = stt + 1});
        }
        #endregion

        #region Update Methods
        [RBACAuthorize(PermissionId = 16)]
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

        [HttpPost]
        public async Task<IActionResult> Update(string id, BoTruyen boTruyen, IFormFile AnhBanner, IFormFile AnhBia)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
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

                    if (AnhBanner != null)
                    {
                        existingProduct.AnhBanner = await SaveImage(AnhBanner);
                    }
                    if (AnhBia != null)
                    {
                        existingProduct.AnhBia = await SaveImage(AnhBia);
                    }

                    existingProduct.TenBo = boTruyen.TenBo;
                    existingProduct.Dotuoi = boTruyen.Dotuoi;
                    existingProduct.IdTg = boTruyen.IdTg;
                    existingProduct.Mota = boTruyen.Mota;
                    existingProduct.TtPemium = boTruyen.TtPemium;
                    existingProduct.TrangThai = boTruyen.TrangThai;
                    existingProduct.Active = boTruyen.Active;
                    existingProduct.Listloai = boTruyen.Listloai;

                    await _botruyenrepository.UpdateAsync(existingProduct);
                    await transaction.CommitAsync();

                    return Json(new { success = true, message = "Update successful!" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
        }
        #endregion

        #region Toggle Methods
        [RBACAuthorize(PermissionId = 18)]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var boTruyen = await _botruyenrepository.GetByIdAsync(id);
            if (boTruyen == null)
            {
                return NotFound();
            }

            boTruyen.Active = !boTruyen.Active;
            await _botruyenrepository.UpdateAsync(boTruyen);
            return RedirectToAction(nameof(Detail), new { id });
        }

        [RBACAuthorize(PermissionId = 18)]
        [HttpPost]
        public async Task<IActionResult> TogglePremium(string id)
        {
            var comic = await _botruyenrepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound("Comic not found.");
            }

            comic.TtPemium = !comic.TtPemium;
            await _botruyenrepository.UpdateAsync(comic);

            var chapters = await _chapterrepository.GetChaptersByComicId(id);
            foreach (var chapter in chapters)
            {
                chapter.TtPemium = comic.TtPemium;
                await _chapterrepository.UpdateAsync(chapter);
            }

            TempData["Message"] = "Comic and all related chapters' premium status updated successfully!";
            return RedirectToAction(nameof(Detail), new { id });
        }
        #endregion

        #region Delete Methods
        [RBACAuthorize(PermissionId = 17)]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var boTruyen = await _botruyenrepository.GetByIdAsync(id);
                    if (boTruyen == null)
                    {
                        return NotFound();
                    }

                    await _botruyenrepository.DeleteAllChaptersAndDetails(id);
                    await _botruyenrepository.DeleteAsync(id);
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(ViewList));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
        }
        #endregion

        #region Validation Methods
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
        #endregion
    }
}
