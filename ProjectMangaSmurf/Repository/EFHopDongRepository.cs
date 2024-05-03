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

        public async Task AddAsync(Payment hd)
        {
             _context.Payments.Add(hd);
            await _context.SaveChangesAsync();
        }

        public string GenerateHD()
        {
            string idPrefix = "HD";
            int idLength = 10;
            string maxId = _context.Payments.Select(kh => kh.IdUser)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "0000000001";
            }
            else
            {
                int currentNumber = int.Parse(maxId.Substring(idPrefix.Length));
                string newId = idPrefix + (currentNumber + 1).ToString().PadLeft(idLength, '0');
                return newId;
            }
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }
    }
}
