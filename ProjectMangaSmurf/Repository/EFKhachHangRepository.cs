using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFKhachHangRepository : IKhachHangRepository
    {
        private readonly ProjectDBContext _context;

        public EFKhachHangRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(KhachHang KhachHang)
        {
            _context.KhachHangs.Add(KhachHang);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(KhachHang id)
        {
            throw new NotImplementedException();
        }

        public string GenerateCustomerId()
        {
            string idPrefix = "KH";
            int idLength = 8;
            string maxId = _context.KhachHangs.Select(kh => kh.IdKh)
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
        public Task<KhachHang> GetAccountByIdAsync(string name, string pass)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<KhachHang> GetByEmailAsync(string id)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(p => p.Email == id.Trim());
        }

        public async Task<KhachHang> GetByIdAsync(string id)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(p => p.Taikhoan == id.Trim());
        }
        public async Task UpdateAsync(KhachHang KhachHang)
        {
             _context.KhachHangs.Update(KhachHang);
            await _context.SaveChangesAsync();
        }
    }
}
