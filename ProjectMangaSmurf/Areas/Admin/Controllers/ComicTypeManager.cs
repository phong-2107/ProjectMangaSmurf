using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class ComicTypeManager : Controller
    {
        private readonly IComicTypeRepository _comicTypeRepository;
        private readonly IboTruyenRepository _botruyen;
        private readonly ProjectDBContext _context;

        public ComicTypeManager(IComicTypeRepository comicTypeRepository, IboTruyenRepository botruyen, ProjectDBContext context)
        {
            _comicTypeRepository = comicTypeRepository;
            _botruyen = botruyen;
            _context = context;
        }

        #region ToggleActive
        [HttpPost]
        [RBACAuthorize(PermissionId = 37)]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var loaiTruyen = await _comicTypeRepository.GetByIdAsync(id);
            if (loaiTruyen != null)
            {
                loaiTruyen.Active = !loaiTruyen.Active; // Toggle the current state
                await _comicTypeRepository.UpdateAsync(loaiTruyen); // Assuming UpdateAsync handles the update operation
                return RedirectToAction("Detail", new { id = loaiTruyen.IdLoai });
            }
            return NotFound();
        }
        #endregion

        #region Index
        [RBACAuthorize(PermissionId = 32)]
        public async Task<IActionResult> Index()
        {
            var comicTypes = await _comicTypeRepository.GetAllAsync();
            return View(comicTypes);
        }
        #endregion

        #region Detail
        [RBACAuthorize(PermissionId = 33)]
        public async Task<IActionResult> Detail(string id)
        {
            var comicType = await _comicTypeRepository.GetLoaiByIdAsync(id);
            if (comicType == null)
            {
                return NotFound();
            }

            var comicSeries = await _botruyen.GetAllAllAsync();
            var relatedComics = comicSeries.Where(c => c.Listloai.Contains(id)).ToList();

            ViewBag.RelatedComics = relatedComics;
            ViewBag.RelatedSeriesCount = relatedComics.Count;
            return View(comicType);
        }
        #endregion

        #region Add
        [RBACAuthorize(PermissionId = 34)]
        public async Task<IActionResult> Add()
        {
            ViewBag.Id = await _comicTypeRepository.GenerateLoaiTruyenId();
            return View(new LoaiTruyen());
        }

        [HttpPost]
        public async Task<IActionResult> Add(LoaiTruyen author)
        {
            if (ModelState.IsValid)
            {
                await _comicTypeRepository.AddAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }
        #endregion

        #region Update
        [RBACAuthorize(PermissionId = 35)]
        public async Task<IActionResult> Update(string id)
        {
            var comicType = await _comicTypeRepository.GetLoaiByIdAsync(id);
            if (comicType == null)
            {
                return NotFound();
            }
            return View(comicType);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, LoaiTruyen comicType)
        {
            if (id != comicType.IdLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _comicTypeRepository.UpdateAsync(comicType);
                return RedirectToAction(nameof(Index));
            }
            return View(comicType);
        }
        #endregion

        #region DeleteConfirmed
        [HttpPost]
        [RBACAuthorize(PermissionId = 36)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _comicTypeRepository.DeleteAsync(id);
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion
    }
}
