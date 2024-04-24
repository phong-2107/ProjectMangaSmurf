using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class AuthorManager : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IboTruyenRepository _botruyen;
        public AuthorManager(IAuthorRepository authorRepository , IboTruyenRepository botruyen)
        {
            _authorRepository = authorRepository;
            _botruyen = botruyen;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var author = await _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }

            // Toggle the active state safely
            author.Active = !(author.Active ?? false);
            await _authorRepository.UpdateAsync(author);

            // Fetch all comics by this author
            var comics = await _botruyen.GetComicsByAuthorId(id);

            // Update all comics' active status to match the author's new status
            if (comics != null)
            {
                if (author.Active == true)
                {
                    foreach (var comic in comics)
                    {
                        comic.Active = true;
                        await _botruyen.UpdateAsync(comic);
                    }
                }
                if (author.Active == false)
                {
                    foreach (var comic in comics)
                    {
                        comic.Active = false;
                        await _botruyen.UpdateAsync(comic);
                    }
                }

            }

            // Optionally add a success message or similar feedback
            TempData["Message"] = "Author and related comics' active status updated successfully!";

            return RedirectToAction(nameof(Detail), new { id = id });
        }


        public async Task<IActionResult> Index(bool? isActive)
        {
            IEnumerable<TacGium> authors;
            if (isActive.HasValue)
            {
                authors = await _authorRepository.GetAllAuthorsFilteredByActiveStatus(isActive.Value);
            }
            else
            {
                authors = await _authorRepository.GetAllAuthors();
            }
            return View(authors);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var author = await _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }

            var relatedComics = await _botruyen.GetComicsByAuthorId(id);
            ViewBag.RelatedComics = relatedComics ?? new List<ProjectMangaSmurf.Models.BoTruyen>(); // Ensure this is never null
            ViewBag.RelatedSeriesCount = relatedComics.Count;

            return View(author);
        }

        public IActionResult Add()
        {
            return View(new TacGium()); // Assuming TacGium is the author model
        }

        [HttpPost]
        public async Task<IActionResult> Add(TacGium author)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.AddAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Update(string id)
        {
            var author = await _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, TacGium author)
        {
            if (id != author.IdTg) // Assuming 'Id' is the identifier in TacGium
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _authorRepository.UpdateAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var author = await _authorRepository.GetAuthorById(id);
            if (author != null)
            {
                await _authorRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
