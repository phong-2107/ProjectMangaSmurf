

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;

using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;




namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class StaffManager : Controller
    {
        private readonly IStaffRepository _Staff;

        public StaffManager(IStaffRepository Nv)
        {
            _Staff = Nv;
        }

        public async Task<IActionResult> ViewList()
        {
            var list = await _Staff.GetAllAsync();
            return View(list); 
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerStatus(string IdNv, bool LoaiNv, bool Active)
        {
            var customer = await _Staff.GetByIdAsync(IdNv);
            if (customer == null)
            {
                return NotFound();
            }

            customer.LoaiNv = LoaiNv;
            customer.Active = Active;
            await _Staff.UpdateAsync(customer);

            return RedirectToAction(nameof(Detail), new { id = IdNv });
        }

        public async Task<IActionResult> Detail(string id)
        {
            var customer = await _Staff.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Update(string id)
        {
            var K = await _Staff.GetByIdAsync(id);
            if (K == null)
            {
                return NotFound();
            }
            return View(K);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, NhanVien khachHang)
        {
            if (id != khachHang.IdNv)
            {
                return NotFound();
            }
            else
            {
                var existingProduct = await _Staff.GetByIdAsync(id);
                existingProduct.Active = khachHang.Active;
                existingProduct.LoaiNv = khachHang.LoaiNv;

                await _Staff.UpdateAsync(existingProduct);

                return RedirectToAction(nameof(Index));
            }
        }

    }
}
