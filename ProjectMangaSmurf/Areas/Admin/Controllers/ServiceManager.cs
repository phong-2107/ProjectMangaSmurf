using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class ServiceManager : Controller
    {
        private readonly IService _ser;
        private readonly ProjectDBContext _context;

        public ServiceManager(IService ser, ProjectDBContext context)
        {
            _ser = ser;
            _context = context;
        }

        #region Index
        [RBACAuthorize(PermissionId = 38)]
        public async Task<IActionResult> Index()
        {
            var list = await _ser.GetQueryV().ToListAsync();
            return View(list);
        }
        #endregion

        #region ToggleActive
        [HttpPost]
        [RBACAuthorize(PermissionId = 43)]
        public async Task<IActionResult> ToggleActive(string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var servicePack = await _context.ServicePackConfigs.FindAsync(id);
                    if (servicePack == null)
                    {
                        return NotFound();
                    }

                    servicePack.Active = !(servicePack.Active ?? false);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Ok();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region Update
        [RBACAuthorize(PermissionId = 41)]
        public async Task<IActionResult> Update(string id)
        {
            var servicePack = await _ser.GetByIdAsync(id);
            if (servicePack == null)
            {
                return NotFound();
            }
            return View(servicePack);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ServicePackConfig servicePackConfig)
        {
            if (id != servicePackConfig.IdPack)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (servicePackConfig.IdPack == "P001" || servicePackConfig.IdPack == "P002" || servicePackConfig.IdPack == "P003")
                        {
                            var existingPack = await _ser.GetByIdAsync(id);
                            if (existingPack == null)
                            {
                                return NotFound();
                            }

                            servicePackConfig.ParentPack = existingPack.ParentPack;
                        }

                        await _ser.UpdateAsync(servicePackConfig);
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
            return View(servicePackConfig);
        }
        #endregion

        #region DeleteConfirmed
        [HttpPost]
        [RBACAuthorize(PermissionId = 42)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == "P001" || id == "P002" || id == "P003")
            {
                return BadRequest("Cannot delete this service pack.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _ser.DeleteAsync(id);
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

        #region Add
        [RBACAuthorize(PermissionId = 40)]
        public async Task<IActionResult> Add()
        {
            ViewBag.Id = await _ser.GenerateId();
            return View(new ServicePackConfig());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServicePackConfig servicePackConfig)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _ser.AddAsync(servicePackConfig);
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
            return View(servicePackConfig);
        }
        #endregion
    }
}
