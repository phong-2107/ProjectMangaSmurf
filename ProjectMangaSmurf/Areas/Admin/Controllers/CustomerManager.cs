using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;

using ProjectMangaSmurf.Models;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models.ViewModels;




namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class CusManager : Controller
    {
        private readonly IKhachHangRepository _khachhangrepository;

        public CusManager(IKhachHangRepository khachHangRepository)

        {
            _khachhangrepository = khachHangRepository;

        }




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
                    query = query.Where(kh => kh.ActiveStats == 0 ); // Banned customers
                    break;
                case "conflict":
                    query = query.Where(kh => kh.ActiveStats == 2); // Banned customers
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

        public async Task<IActionResult> ViewOrderList()
        {
            var orders = await _khachhangrepository.GetAllPaymentAsync();  // Assuming the repository method name
            return View(orders);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCustomerStatus(string IdKh, bool TtPremium, byte Active)
        {
            var customer = await _khachhangrepository.GetByIdAsync(IdKh);
            if (customer == null)
            {
                return NotFound();
            }

            customer.ActivePremium = TtPremium;
            customer.ActiveStats = Active;
            await _khachhangrepository.UpdateAsync(customer);

            return RedirectToAction(nameof(Detail), new { id = IdKh });
        }

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



        public async Task<IActionResult> OrderDetail(string id)
        {
            var order = await _khachhangrepository.GetAllIdHDAsync(id);
            if (order == null || !order.Any()) // Check for null or empty collection
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> Update(string id)
        {
            var K = await _khachhangrepository.GetByIdAsync(id);
            if (K == null)
            {
                return NotFound();
            }
            return View(K);
        }
         
        [HttpPost]
        public async Task<IActionResult> Update(string id, KhachHang khachHang)
        {
            if (id != khachHang.IdUser)
            {
                return NotFound();
            }
            else
            {
                var existingProduct = await _khachhangrepository.GetByIdAsync(id);
                existingProduct.ActiveStats = khachHang.ActiveStats;
                existingProduct.ActivePremium = khachHang.ActivePremium;

                await _khachhangrepository.UpdateAsync(existingProduct);

                return RedirectToAction(nameof(Index));
            }
        }





    }
}
