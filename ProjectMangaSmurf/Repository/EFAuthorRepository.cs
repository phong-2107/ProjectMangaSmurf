
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectMangaSmurf.Repository
{ 
    public class EFAuthorRepository : IAuthorRepository
    {
        private readonly ProjectDBContext _context;

        public EFAuthorRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TacGium>> GetAllAuthors()
        {
            return await _context.TacGia.ToListAsync();
        }

        public async Task<TacGium> GetAuthorById(string id)
        {
            return await _context.TacGia.FindAsync(id);
        }

        public async Task AddAsync(TacGium ath)
        {
            _context.TacGia.Add(ath);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TacGium ath)
        {
            _context.Entry(ath).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<TacGium>> GetAllAuthorsFilteredByActiveStatus(bool isActive)
        {
            return await _context.TacGia.Where(a => a.Active == isActive).ToListAsync();
        }
        public async Task<List<BoTruyen>> GetComicsByAuthorId(string authorId)
        {
            return await _context.BoTruyens.Where(c => c.IdTg == authorId).ToListAsync();
        }

        public async Task DeleteAllChaptersAndDetails(string idBo)
        {
            var comicl = await _context.BoTruyens
                .Include(lh => lh.CtLoaiTruyens)
                        .FirstOrDefaultAsync(c => c.IdBo == idBo);
            var comic = await _context.BoTruyens
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.CtChapters)
                        .FirstOrDefaultAsync(c => c.IdBo == idBo);

            if (comicl != null)
            {
                foreach (var chapter in comicl.CtLoaiTruyens)
                {
                    _context.CtLoaiTruyens.Remove(chapter);
                }
            }

            if (comic != null)
            {
                foreach (var chapter in comic.Chapters)
                {
                    foreach (var detail in chapter.CtChapters)
                    {
                        _context.CtChapters.Remove(detail);
                    }
                    _context.Chapters.Remove(chapter);
                }
                _context.BoTruyens.Remove(comic);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var author = await _context.TacGia.FindAsync(id);
            if (author != null)
            {
                _context.TacGia.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateAuthorId()
        {
            string idPrefix = "TG";
            int idLength = 4;
            string maxId = _context.TacGia.Select(kh => kh.IdTg)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "0001";
            }
            else
            {
                int currentNumber = int.Parse(maxId.Substring(idPrefix.Length));
                string newId = idPrefix + (currentNumber + 1).ToString().PadLeft(idLength, '0');
                return newId;
            }
        }
    }
}
