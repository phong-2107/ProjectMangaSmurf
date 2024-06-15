using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class AuthorManager : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IboTruyenRepository _botruyen;
        private readonly ProjectDBContext _context;

        public AuthorManager(IAuthorRepository authorRepository, IboTruyenRepository botruyen, ProjectDBContext context)
        {
            _authorRepository = authorRepository;
            _botruyen = botruyen;
            _context = context;
        }

        #region ToggleActive
        [HttpPost]
        [RBACAuthorize(PermissionId = 31)]
        public async Task<IActionResult> ToggleActive(string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var author = await _authorRepository.GetAuthorById(id);
                    if (author == null)
                    {
                        return NotFound();
                    }

                    // Toggle the active state safely
                    author.Active = !(author.Active ?? false); // Use ?? to provide a default value
                    await _authorRepository.UpdateAsync(author);

                    // Fetch all comics by this author
                    var comics = await _botruyen.GetComicsByAuthorId(id);

                    // Update all comics' active status to match the author's new status
                    if (comics != null)
                    {
                        foreach (var comic in comics)
                        {
                            comic.Active = author.Active ?? false; // Use ?? to provide a default value
                            await _botruyen.UpdateAsync(comic);
                        }
                    }

                    await transaction.CommitAsync();

                    // Optionally add a success message or similar feedback
                    TempData["Message"] = "Author and related comics' active status updated successfully!";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                }

                return RedirectToAction(nameof(Detail), new { id });
            }
        }
        #endregion

        #region Index
        [RBACAuthorize(PermissionId = 26)]
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
        #endregion

        #region Detail
        [RBACAuthorize(PermissionId = 27)]
        public async Task<IActionResult> Detail(string id)
        {
            var author = await _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }

            var relatedComics = await _botruyen.GetComicsByAuthorId(id);
            ViewBag.RelatedComics = relatedComics ?? new List<BoTruyen>(); // Ensure this is never null
            ViewBag.RelatedSeriesCount = relatedComics.Count;

            return View(author);
        }
        #endregion

        #region Add
        [RBACAuthorize(PermissionId = 28)]
        public async Task<IActionResult> Add()
        {
            ViewBag.Id = await _authorRepository.GenerateAuthorId(); // Call without any arguments
            return View(new TacGium()); // Assuming TacGium is the author model
        }

        [HttpPost]
        [RBACAuthorize(PermissionId = 28)]
        public async Task<IActionResult> Add(TacGium author)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _authorRepository.AddAsync(author);
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "An error occurred: " + ex.Message);
                    }
                }
            }
            return View(author);
        }
        #endregion

        #region Update
        [RBACAuthorize(PermissionId = 29)]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var author = await _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [RBACAuthorize(PermissionId = 29)]
        public async Task<IActionResult> Update(TacGium author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if Active status is being set to False
                    if (author.Active == false)
                    {
                        // Fetch all comics by this author
                        var comics = await _botruyen.GetComicsByAuthorId(author.IdTg);

                        // Update all comics' active status to match the author's new status
                        if (comics != null)
                        {
                            foreach (var comic in comics)
                            {
                                comic.Active = false;
                                await _botruyen.UpdateAsync(comic);
                            }
                        }
                    }

                    await _authorRepository.UpdateAsync(author);
                    await transaction.CommitAsync();

                    TempData["Message"] = "Author updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }

            return View(author);
        }
        #endregion

        #region Delete
        [HttpPost]
        [RBACAuthorize(PermissionId = 30)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var author = await _authorRepository.GetAuthorById(id);
                    if (author == null)
                    {
                        return NotFound();
                    }

                    // Fetch all comics by this author
                    var comics = await _botruyen.GetComicsByAuthorId(id);
                    if (comics != null)
                    {
                        foreach (var comic in comics)
                        {
                            // Assuming a method to delete all chapters and details of a comic
                            await _authorRepository.DeleteAllChaptersAndDetails(comic.IdBo);
                            // Now delete the comic
                            await _authorRepository.DeleteAsync(comic.IdBo);
                        }
                    }

                    // Finally, delete the author
                    await _authorRepository.DeleteAsync(id);
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
        }
        [RBACAuthorize(PermissionId = 30)]
        public async Task<IActionResult> Delete(string id)
        {
            var author = await _authorRepository.GetAuthorById(id);
            if (author != null)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _authorRepository.DeleteAsync(id);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "An error occurred: " + ex.Message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
        #endregion

        #region Private Methods
        private bool AuthorExists(string id)
        {
            return _authorRepository.GetAuthorById(id) != null;
        }
        #endregion


    }
}
