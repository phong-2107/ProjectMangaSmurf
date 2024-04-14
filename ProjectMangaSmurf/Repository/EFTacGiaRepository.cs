using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFTacGiaRepository : ITacGiaRepository
    {
        private readonly ProjectDBContext _context;
        public EFTacGiaRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TacGium>> GetAllAsync()
        {
            return await _context.TacGia.ToListAsync();
        }
    }
}
