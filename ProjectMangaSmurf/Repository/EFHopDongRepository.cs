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
            int idLength = 8;
            string maxId = _context.Payments.Select(kh => kh.IdPayment)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "00000001";
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

        public ServicePackConfig GetPackById(string id)
        {
            return _context.ServicePackConfigs.FirstOrDefault(p => p.IdPack == id);
        }

        public async Task<ServicePackConfig> GetPackByIdAsync(string id)
        {
            return await _context.ServicePackConfigs.FirstOrDefaultAsync(p => p.IdPack == id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentByIdAsync(string id)
        {
            return await _context.Payments.Where(p => p.IdUser == id).ToListAsync();
        }
    }
}
