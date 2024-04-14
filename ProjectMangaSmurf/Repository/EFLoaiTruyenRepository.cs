using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFLoaiTruyenRepository : ILoaiTruyenRepository
    {
        private readonly ProjectDBContext _context;

        public EFLoaiTruyenRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task AddAsyncCTLoai(CtLoaiTruyen ctloai)
        {
            _context.CtLoaiTruyens.Add(ctloai);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsyncLoai(LoaiTruyen loai)
        {
            _context.LoaiTruyens.Add(loai);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LoaiTruyen>> GetAllAsync()
        {
            return await _context.LoaiTruyens.ToListAsync();
        }
    }
}
