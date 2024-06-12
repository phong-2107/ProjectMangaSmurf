using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFCTBoTruyenRepository : ICTBoTruyenRepository
    {
        private readonly ProjectDBContext _context;

        public EFCTBoTruyenRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(CtBoTruyen botruyen)
        {
            _context.CtBoTruyens.Add(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task<double> CountAllAsyncByBoTruyen(string id)
        {

            var reviews = _context.CtBoTruyens
                          .Where(p => p.IbBo == id && p.DanhGia != 0);

            if (await reviews.AnyAsync())
            {
                double totalRating = await reviews.SumAsync(c => (double)c.DanhGia);
                int count = await reviews.CountAsync();
                return totalRating / count;
            }

            return 0.0;
        }

        public async Task DeleteAsync(string id)
        {
            var botruyen = await _context.BoTruyens.FindAsync(id);
            botruyen.Active = false;
            _context.BoTruyens.Update(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CtBoTruyen>> GetAllAsyncByBoTruyen(string id)
        {
            return await _context.CtBoTruyens.Where(p => p.IbBo == id).ToListAsync();
        }

        public async Task<IEnumerable<CtBoTruyen>> GetAllAsyncByID(string id)
        {
            return await _context.CtBoTruyens.Where(p => p.IdUser == id).ToListAsync();
        }

        public async Task<IEnumerable<CtBoTruyen>> GetAllAsyncFollowByID(string id)
        {
            return await _context.CtBoTruyens.Where(p => (p.IdUser == id) && (p.Theodoi == true)).ToListAsync();
        }

        public async Task<IEnumerable<CtBoTruyen>> GetAllAsyncHistoryByID(string id)
        {
            return await _context.CtBoTruyens.Where(p => (p.IdUser == id) && (p.LsMoi != "-1")).ToListAsync();
        }

        public async Task<CtBoTruyen> GetByIdAsync(string IdUser, string idbo)
        {
            return await _context.CtBoTruyens.FirstOrDefaultAsync(p => (p.IdUser == IdUser) && (p.IbBo == idbo));
        }

        public async Task<CtBoTruyen> GetByIdFollowAsync(string IdUser, string idbo)
        {
            return await _context.CtBoTruyens.FirstOrDefaultAsync(p => (p.IdUser == IdUser) && (p.IbBo == idbo) && (p.Theodoi == true));
        }

        public async Task UpdateAsync(CtBoTruyen botruyen)
        {
            _context.CtBoTruyens.Update(botruyen);
            await _context.SaveChangesAsync();
        }
    }
}