using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ComicTypeManager : Controller
    {
        private readonly IComicTypeRepository _comicTypeRepository;
        private readonly IboTruyenRepository _botruyen;

        public ComicTypeManager(IComicTypeRepository comicTypeRepository, IboTruyenRepository botruyen)
        {
            _comicTypeRepository = comicTypeRepository;
            _botruyen = botruyen;  
        }

        [HttpPost]
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


        // List all comic types
        public async Task<IActionResult> Index()
        {
            var comicTypes = await _comicTypeRepository.GetAllAsync();
            return View(comicTypes);
        }

        // Display details of a specific comic type
        public async Task<IActionResult> Detail(string id)
        {
            var comicType = await _comicTypeRepository.GetLoaiByIdAsync(id);
            if (comicType == null)
            {
                return NotFound();
            }

            // Fetching all comic series that belong to this type
            var comicSeries = await _botruyen.GetAllAllAsync();
            var relatedComics = comicSeries.Where(c => c.listloai.Contains(id)).ToList();

            ViewBag.RelatedComics = relatedComics;
            ViewBag.RelatedSeriesCount = relatedComics.Count;
            return View(comicType);
        }


        // Display form to add a new comic type
        public IActionResult Add()
        {
            return View();
        }

        // Handle adding a new comic type
        [HttpPost]
        public async Task<IActionResult> Add(LoaiTruyen comicType)
        {
            if (ModelState.IsValid)
            {
                await _comicTypeRepository.AddAsync(comicType);
                return RedirectToAction(nameof(Index));
            }
            return View(comicType);
        }

        // Display form to update a comic type
        public async Task<IActionResult> Update(string id)
        {
            var comicType = await _comicTypeRepository.GetLoaiByIdAsync(id);
            if (comicType == null)
            {
                return NotFound();
            }
            return View(comicType);
        }

        // Handle updating a comic type
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

        // Handle deletion of a comic type
        public async Task<IActionResult> Delete(string id)
        {
            var comicType = await _comicTypeRepository.GetLoaiByIdAsync(id);
            if (comicType != null)
            {
                await _comicTypeRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
