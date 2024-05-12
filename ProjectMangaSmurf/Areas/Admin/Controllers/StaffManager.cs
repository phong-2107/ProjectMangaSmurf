using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
            IEnumerable<NhanVien> staffList = await _staffRepository.GetAllAsyncStaff() ?? Enumerable.Empty<NhanVien>();
            return View(staffList);
        }

        public async Task<IActionResult> ViewList()
        {
            var list = await _staffRepository.GetAllAsync(); // Assumes GetAllAsync only returns active staff
            return View(list);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
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
