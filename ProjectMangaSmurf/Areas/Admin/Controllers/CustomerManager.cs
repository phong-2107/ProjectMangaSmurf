using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Models.ViewModels;
using ProjectMangaSmurf.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class CusManager : Controller
    {
        private readonly IKhachHangRepository _khachhangrepository;
        private readonly ProjectDBContext _context;

        public CusManager(IKhachHangRepository khachHangRepository, ProjectDBContext context)
        {
            _khachhangrepository = khachHangRepository;
            _context = context;
        }

        #region ViewList
        [RBACAuthorize(PermissionId = 44)]
        public async Task<IActionResult> ViewList(string status = "all", bool premium = false)
        {
            IQueryable<KhachHang> query = _khachhangrepository.GetQuery();  // Get base query from the repository

            // Apply status filters
            switch (status)
            {
                case "active":
                    query = query.Where(kh => kh.ActiveStats == 1);  // Active customers
                    break;
                case "banned":
                    query = query.Where(kh => kh.ActiveStats == 0); // Banned customers
                    break;
                case "conflict":
                    query = query.Where(kh => kh.ActiveStats == 2); // Conflict customers
                    break;
            }

            // Apply premium filter if checked
            if (premium)
            {
                query = query.Where(kh => kh.ActivePremium == true); // Premium status customers
            }

            var list = await query.ToListAsync(); // Execute the query and get the list
            return View(list); // Pass the list to the view
        }
        #endregion

        #region ViewOrderList
        [RBACAuthorize(PermissionId = 44)]
        public async Task<IActionResult> ViewOrderList(DateTime? startDate, DateTime? endDate, int? month, int? year, int? quarter, int? quarterYear)
        {
            IQueryable<Payment> paymentsQuery = _context.Payments.Include(p => p.IdUserNavigation.IdUserNavigation);

            if (startDate.HasValue && endDate.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(p => p.PayDate >= startDate && p.PayDate <= endDate);
            }
            else if (month.HasValue && year.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(p => p.PayDate.Value.Month == month && p.PayDate.Value.Year == year);
            }
            else if (year.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(p => p.PayDate.Value.Year == year);
            }
            else if (quarter.HasValue && quarterYear.HasValue)
            {
                int startMonth = (quarter.Value - 1) * 3 + 1;
                int endMonth = startMonth + 2;
                paymentsQuery = paymentsQuery.Where(p => p.PayDate.Value.Year == quarterYear && p.PayDate.Value.Month >= startMonth && p.PayDate.Value.Month <= endMonth);
            }

            var payments = await paymentsQuery.ToListAsync();
            return View(payments);
        }

        #endregion

        #region UpdateCustomerStatus
        [HttpPost]
        [RBACAuthorize(PermissionId = 48)]
        public async Task<IActionResult> UpdateCustomerStatus(string IdKh, bool TtPremium, byte Active)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var customer = await _khachhangrepository.GetByIdAsync(IdKh);
                    if (customer == null)
                    {
                        return NotFound();
                    }

                    customer.ActivePremium = TtPremium;
                    customer.ActiveStats = Active;
                    await _khachhangrepository.UpdateAsync(customer);
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Detail), new { id = IdKh });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                }
            }

            return RedirectToAction(nameof(Detail), new { id = IdKh });
        }
        #endregion

        #region Detail
        [RBACAuthorize(PermissionId = 45)]
        public async Task<IActionResult> Detail(string id)
        {
            var customer = await _khachhangrepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var orders = await _khachhangrepository.GetAllIdHDAsync(id); // Assuming this returns IEnumerable<HopDong>

            var viewModel = new CustomerDetailsViewModel
            {
                Customer = customer,
                Orders = orders
            };

            return View(viewModel);
        }
        #endregion

        [RBACAuthorize(PermissionId = 45)]
        #region OrderDetail
        public async Task<IActionResult> OrderDetail(string id)
        {
            var order = await _khachhangrepository.GetAllIdHDAsync(id);
            if (order == null || !order.Any()) // Check for null or empty collection
            {
                return NotFound();
            }
            return View(order);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(string id)
        {
            var K = await _khachhangrepository.GetByIdAsync(id);
            if (K == null)
            {
                return NotFound();
            }
            return View(K);
        }

        [RBACAuthorize(PermissionId = 47)]
        [HttpPost]
        public async Task<IActionResult> Update(string id, KhachHang khachHang)
        {
            if (id != khachHang.IdUser)
            {
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingCustomer = await _khachhangrepository.GetByIdAsync(id);
                    existingCustomer.ActiveStats = khachHang.ActiveStats;
                    existingCustomer.ActivePremium = khachHang.ActivePremium;

                    await _khachhangrepository.UpdateAsync(existingCustomer);
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(ViewList));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                }
            }

            return View(khachHang);
        }
        #endregion
    }
}
