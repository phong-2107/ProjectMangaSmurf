using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models.ViewModels;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class StaffManager : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;

        public StaffManager(IStaffRepository staffRepository, IUserRepository userRepository)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> RBAC()
        {
            IEnumerable<PermissionsList> permissionsList = await _staffRepository.GetAllRBACAsync() ?? Enumerable.Empty<PermissionsList>();
            return View(permissionsList);
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<NhanVien> query = _staffRepository.GetQuery();  // Get base query from the repository

            var list = await query.ToListAsync(); // Execute the query and get the list
            return View(list); // Pass the list to the view
        }
        public async Task<IActionResult> Detail(string id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var viewModel = new StaffDetailsViewModel
            {
                Staff = staff
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Update(string id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }
        // Example usage in setting permission statuses
        private async Task UpdatePermissionStatus(byte id, byte status)
        {
            var permission = await _staffRepository.GetPermissionByIdAsync(id);
            if (permission != null)
            {
                permission.PermissionsStats = status;
                await _staffRepository.UpdatePermissionAsync(permission);
            }
        }

        // Controller actions to set specific statuses
        [HttpPost]
        public async Task<IActionResult> SetActive(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)1);  // Active status
            return RedirectToAction("RBAC");
        }

        [HttpPost]
        public async Task<IActionResult> SetInactive(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)0);  // Inactive status
            return RedirectToAction("RBAC");
        }

        [HttpPost]
        public async Task<IActionResult> SetMaintenance(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)2);  // Maintenance status
            return RedirectToAction("RBAC");
        }

        [HttpPost]
        public async Task<IActionResult> SetStop(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)3);  // Stop status
            return RedirectToAction("RBAC");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Update(string id, NhanVien staffUpdate)
        ////{
        ////    if (id != staffUpdate.IdUser)
        ////    {
        ////        return NotFound();
        ////    }

        ////    await _staffRepository.UpdateAsync(staffUpdate);
        ////    return RedirectToAction(nameof(Detail), new { id = staffUpdate.IdUser });
        ////}
    }
}
