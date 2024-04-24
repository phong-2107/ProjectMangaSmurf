using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFHopDongRepository : IHopdongRepository
    {
        private readonly ProjectDBContext _context;

        public EFHopDongRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<HopDong>> GetAllAsync()
        {
            return await _context.HopDongs.ToListAsync();
        }
    }
}
