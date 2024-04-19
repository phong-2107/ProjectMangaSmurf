using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFChapterRepository : IChapterRepository
    {
        private readonly ProjectDBContext _context;

        public EFChapterRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
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

        public Chapter GetChapterEarliestByIdAsync(string id)
        {
            var list = _context.Chapters.Where(p => p.IdBo == id).ToList();
            if (list.Count > 0)
            {
                var latest = list.Max(a => a.SttChap);
                return _context.Chapters.FirstOrDefault(p => p.SttChap == latest);
            }
            else
            {
                return null;
            }
        }

        public int GetEarliestByIdAsync(string id)
        {
            var list = _context.Chapters.Where(p => p.IdBo == id).ToList();
            if(list.Count > 0)
            {
                var latest = list.Max(a => a.SttChap);
                return latest;
            }
            else
            {
                return 0;
            }
        }

        public string CalculateTimeAgo(DateTime releaseDate)
        {
            TimeSpan timeSinceRelease = DateTime.Now - releaseDate;

            if (timeSinceRelease.TotalHours < 1)
            {
                return $"{timeSinceRelease.TotalMinutes:N0} phút trước";
            }
            else if (timeSinceRelease.TotalDays < 1)
            {
                return $"{timeSinceRelease.TotalHours:N0} giờ trước";
            }
            else if (timeSinceRelease.TotalDays < 7)
            {
                return $"{timeSinceRelease.TotalDays:N0} ngày trước";
            }
            else if (timeSinceRelease.TotalDays < 30)
            {
                return $"{timeSinceRelease.TotalDays / 7:N0} tuần trước";
            }
            else
            {
                return $"{timeSinceRelease.TotalDays / 30:N0} tháng trước";
            }
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsyncCT(CtChapter CTs)
        {
            _context.CtChapters.Add(CTs);
            await _context.SaveChangesAsync();
        }
    }
}
