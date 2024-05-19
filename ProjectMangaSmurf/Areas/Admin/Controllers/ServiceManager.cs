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

        public async Task<IActionResult> Index()
        {
            var list = await _ser.GetQueryV().ToListAsync();
            return View(list);
        }
    }
}
