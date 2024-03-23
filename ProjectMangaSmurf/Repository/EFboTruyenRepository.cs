using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFboTruyenRepository : IboTruyenRepository
    {
        private readonly MangaSmurfContext _context;

        public EFboTruyenRepository(MangaSmurfContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BoTruyen>> GetAllAsync()
        {
            return await _context.BoTruyens.ToListAsync();
        }

        public async Task<BoTruyen> GetByIdAsync(string id)
        {
            return await _context.BoTruyens.Include(p => p.CtBoTruyens).FirstOrDefaultAsync(p => p.IdBo == id);
        }

        public async Task AddAsync(BoTruyen botruyen)
        {
            _context.BoTruyens.Add(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BoTruyen botruyen)
        {
            _context.BoTruyens.Update(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var botruyen = await _context.BoTruyens.FindAsync(id);
            _context.BoTruyens.Remove(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BoTruyen>> GetAllByTopic(string name)
        {
            var getid = _context.LoaiTruyens.FirstOrDefault(p => p.TenLoai.Trim().Equals(name.Trim()));
            var getListTruyen = _context.BoTruyens
            return await _context.BoTruyens.Where(p => p.IdLoai.Equals(getid.IdLoai)).ToListAsync();
        }



        public async Task<LoaiTruyen> GetLoaiByIdAsync(string id)
        {
            return await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.IdLoai == id);
        }
    }
}
