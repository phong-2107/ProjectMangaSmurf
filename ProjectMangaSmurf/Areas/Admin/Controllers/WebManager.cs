using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Models.ViewModels;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WebManagerController : Controller
    {
        private readonly ProjectDBContext _context;
        private readonly ILogger<WebManagerController> _logger;

        public WebManagerController(ProjectDBContext context, ILogger<WebManagerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Edit()
        {
            try
            {
                // Directly fetch the first or default record assuming only one record is present
                var footer = _context.Footers.FirstOrDefault() ?? new Footer();
                var premium = _context.Premia.FirstOrDefault() ?? new Premium();

                var viewModel = new EditViewModel
                {
                    Footer = footer,
                    Premium = premium
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to load data for edit: {Message}", ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Assume only one record exists, and directly update or create it
                var existingFooter = _context.Footers.FirstOrDefault();
                var existingPremium = _context.Premia.FirstOrDefault();

                if (existingFooter != null)
                {
                    _context.Entry(existingFooter).CurrentValues.SetValues(model.Footer);
                }
                else
                {
                    _context.Footers.Add(model.Footer);
                }

                if (existingPremium != null)
                {
                    _context.Entry(existingPremium).CurrentValues.SetValues(model.Premium);
                }
                else
                {
                    _context.Premia.Add(model.Premium);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update: {Message}", ex.Message);
                return View("Error");
            }
        }
    }
}
