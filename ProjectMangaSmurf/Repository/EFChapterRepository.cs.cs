using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFChapterRepository : IChapterRepository
    {
        private readonly MangaSmurfContext _context;

        public EFChapterRepository(MangaSmurfContext context)
        {
            _context = context;
        }
        public Task AddAsync(Chapter botruyen)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _context.Chapters.ToListAsync();
        }

        public async Task<IEnumerable<Chapter>> GetAllByIdAsync(string id)
        {
            return await _context.Chapters.Where(p => p.IdBo == id).ToListAsync();
        }

        public async Task<IEnumerable<CtChapter>> GetAllCTByIdAsync(string id, int stt)
        {
            return await _context.CtChapters.Where(p => (p.IdBo == id) && (p.SttChap == stt)).ToListAsync();
        }

        public async Task<Chapter> GetByIdAsync(string id, int stt)
        {
            return await _context.Chapters.FirstOrDefaultAsync(p => (p.IdBo == id) && (p.SttChap == stt));
        }

        public Task UpdateAsync(Chapter botruyen)
        {
            throw new NotImplementedException();
        }
    }
}
