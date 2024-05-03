using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Helper;
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
        public async Task<List<Chapter>> GetChaptersByComicId(string comicId)
        {
            if (string.IsNullOrEmpty(comicId))
                return new List<Chapter>(); 

            return await _context.Chapters
                .Where(chapter => chapter.IdBo == comicId)
                .OrderBy(chapter => chapter.SttChap) 
                .ToListAsync();
        }
        public async Task<CtChapter> GetCTChapterByIdAsync(string idBo, int sttChap, int soTrang)
        {
            return await _context.CtChapters.FirstOrDefaultAsync(c => c.IdBo == idBo && c.SttChap == sttChap && c.SoTrang == soTrang);
        }

        public async Task UpdateCTChapterAsync(CtChapter ctChapter)
        {
            _context.CtChapters.Update(ctChapter);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsyncCTHD(CtHoatDong ct)
        {
            var find = await _context.CtHoatDongs.FirstOrDefaultAsync(p => (p.IdBo == ct.IdBo) && (p.SttChap == ct.SttChap) && (p.IdUser == ct.IdUser));
            if (find == null)
            {
                _context.CtHoatDongs.Add(ct);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<CtChapter> GetPageByIdAsync(string idBo, int sttChap, int soTrang)
        {
            return await _context.CtChapters
                                 .FirstOrDefaultAsync(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang == soTrang);
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task UpdatePageNumberAsync(string idBo, int sttChap, int currentSoTrang, int newSoTrang)
        {
            var page = await GetPageByIdAsync(idBo, sttChap, currentSoTrang);
            if (page != null)
            {
                // Check if the new page number exists
                var pageExists = await _context.CtChapters.AnyAsync(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang == newSoTrang);
                if (!pageExists)
                {
                    page.SoTrang = newSoTrang;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("A page with the new page number already exists.");
                }
            }
        }

        public async Task DeletePageAsync(string idBo, int sttChap, int soTrang)
        {
            var page = await GetPageByIdAsync(idBo, sttChap, soTrang);
            if (page != null)
            {
                _context.CtChapters.Remove(page);
                await _context.SaveChangesAsync();

                // Update subsequent pages
                var subsequentPages = _context.CtChapters
                                              .Where(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang > soTrang)
                                              .OrderBy(p => p.SoTrang);
                foreach (var subsequentPage in subsequentPages)
                {
                    subsequentPage.SoTrang--;
                }
                await _context.SaveChangesAsync();
            }
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
            // Check if the chapter exists in the database
            var existingChapter = await _context.Chapters
                .FirstOrDefaultAsync(c => c.IdBo == chapter.IdBo && c.SttChap == chapter.SttChap);

            if (existingChapter != null)
            {
                // Update properties
                existingChapter.TenChap = chapter.TenChap;
                existingChapter.ThoiGian = chapter.ThoiGian;
                existingChapter.TkLuotxem = chapter.TkLuotxem;
                existingChapter.TtPemium = chapter.TtPemium;
                existingChapter.Active = chapter.Active;

                // Apply the changes to the database
                _context.Chapters.Update(existingChapter);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Chapter not found.");
            }
        }


        public async Task AddAsyncCT(CtChapter CTs)
        {
            _context.CtChapters.Add(CTs);
            await _context.SaveChangesAsync();
        }
        // Inside your EFChapterRepository or similar repository class
        public async Task<int> GetMaxChapterNumberAsync(string idBo)
        {
            return await _context.Chapters
                                 .Where(c => c.IdBo == idBo)
                                 .MaxAsync(c => (int?)c.SttChap) ?? 0;
        }

        //---------------------------------------------------------------------------
        public async Task ToggleActiveStatus(string idBo, int sttChap, int soTrang)
        {
            var page = await _context.CtChapters
                .FirstOrDefaultAsync(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang == soTrang);
            if (page != null)
            {
                page.Active = !page.Active;
                await _context.SaveChangesAsync();
            }
        }
        public async Task ReplaceImageAsync(string idBo, int sttChap, int soTrang, IFormFile newImage)
        {
            var page = await _context.CtChapters
                .FirstOrDefaultAsync(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang == soTrang);
            if (page != null && newImage != null)
            {
                // Assuming a method to handle the file storage and returning the file path
                var filePath = await FileStorageHelper.SaveFile(newImage);
                page.AnhTrang = filePath;  // Update the image path
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePageAndUpdateSubsequentAsync(string idBo, int sttChap, int soTrang)
        {
            var page = await _context.CtChapters
                .FirstOrDefaultAsync(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang == soTrang);
            if (page != null)
            {
                _context.CtChapters.Remove(page);
                await _context.SaveChangesAsync();

                // Update subsequent pages
                var subsequentPages = await _context.CtChapters
                    .Where(p => p.IdBo == idBo && p.SttChap == sttChap && p.SoTrang > soTrang)
                    .OrderBy(p => p.SoTrang)
                    .ToListAsync();
                foreach (var subsequentPage in subsequentPages)
                {
                    subsequentPage.SoTrang--;
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetMaxPageNumber(string idBo, int sttChap)
        {
            var maxPageNumber = await _context.CtChapters
                .Where(p => p.IdBo == idBo && p.SttChap == sttChap)
                .MaxAsync(p => (int?)p.SoTrang) ?? 0; // Return 0 if no pages found
            return maxPageNumber;
        }
        public async Task DeleteCTChapterAsync(CtChapter ctChapter)
        {
            if (ctChapter == null)
            {
                throw new ArgumentNullException(nameof(ctChapter), "Provided chapter detail is null");
            }

            _context.CtChapters.Remove(ctChapter);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteChapterAsync(Chapter chapter)
        {
            if (chapter == null)
            {
                throw new ArgumentNullException(nameof(chapter), "Provided chapter is null");
            }

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
        }

        // In EFChapterRepository class
        public async Task<IEnumerable<CtHoatDong>> GetAllCtHoatDongByIdAsync(string idBo, int sttChap)
        {
            return await _context.CtHoatDongs
                                 .Where(hd => hd.IdBo == idBo && hd.SttChap == sttChap)
                                 .ToListAsync();
        }
        // In EFChapterRepository class
        public async Task DeleteCtHoatDongAsync(CtHoatDong ctHoatDong)
        {
            if (ctHoatDong == null)
                throw new ArgumentNullException(nameof(ctHoatDong));

            _context.CtHoatDongs.Remove(ctHoatDong);
            await _context.SaveChangesAsync();
        }

    }
}
