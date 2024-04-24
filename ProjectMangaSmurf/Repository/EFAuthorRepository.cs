
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

        public async Task DeleteAsync(string id)
        {
            var author = await _context.TacGia.FindAsync(id);
            if (author != null)
            {
                _context.TacGia.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        public string GenerateAuthorId()
        {
            string idPrefix = "A";
            int idLength = 8;
            string maxId = _context.BoTruyens.Select(kh => kh.IdBo)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "00001";
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
