//using Microsoft.AspNetCore.Mvc;
//using ProjectMangaSmurf.Data;
//using ProjectMangaSmurf.Models;
//using ProjectMangaSmurf.Models.ViewModels;
//using Microsoft.Extensions.Logging;
//using System.Linq;

//namespace ProjectMangaSmurf.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class WebManagerController : Controller
//    {
//        private readonly ProjectDBContext _context;
//        private readonly ILogger<WebManagerController> _logger;

//        public WebManagerController(ProjectDBContext context, ILogger<WebManagerController> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        // GET: Displays existing data to be edited
//        public IActionResult Edit()
//        {
//            try
//            {
//                // Fetch existing records to display for editing
//                var footer = _context.Footers.FirstOrDefault();
//                var premium = _context.Premia.FirstOrDefault();

//                var viewModel = new EditViewModel
//                {
//                    Footer = footer ?? new Footer(),
//                    Premium = premium ?? new Premium()
//                };

//                return View(viewModel);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Failed to load data for edit: {Message}", ex.Message);
//                return View("Error");
//            }
//        }

//        // POST: Updates the data by first removing all entries and adding new ones
//        [HttpPost]
//        public IActionResult Edit(EditViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            using (var transaction = _context.Database.BeginTransaction())
//            {
//                try
//                {
//                    // Delete all existing records
//                    var footers = _context.Footers.ToList();
//                    _context.Footers.RemoveRange(footers);

//                    var premia = _context.Premia.ToList();
//                    _context.Premia.RemoveRange(premia);

//                    _context.SaveChanges(); // Ensure deletion is completed before adding new entries

//                    // Add new records from the provided model data
//                    _context.Footers.Add(model.Footer);
//                    _context.Premia.Add(model.Premium);

//                    _context.SaveChanges(); // Save new entries
//                    transaction.Commit();

//                    return RedirectToAction("Index", "BotruyenManager");
//                }
//                catch (Exception ex)
//                {
//                    transaction.Rollback();
//                    _logger.LogError("Failed to update with new data: {Message}", ex.Message);
//                    return View("Error");
//                }
//            }
//        }
//    }
//}
