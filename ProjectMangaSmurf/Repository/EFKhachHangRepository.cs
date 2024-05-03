using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Linq.Expressions;

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
            try
            {
                _context.KhachHangs.Add(KhachHang);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new NotImplementedException(); }
        }
        public IQueryable<KhachHang> GetQuery()
        {
            return _context.KhachHangs;
        }



        // Sửa lại 
        public async Task DeleteAsync(KhachHang id)
        {
            try
            {
                var n = await _context.KhachHangs.FindAsync(id);
                n.IdUserNavigation.Active = false;
                _context.KhachHangs.Update(id);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new NotImplementedException(); }
        }

        public string GenerateCustomerId()
        {
            string idPrefix = "KH";
            int idLength = 8;
            string maxId = _context.Users.Select(kh => kh.IdUser)
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

        public async Task<IEnumerable<KhachHang>> GetAllAsync(Expression<Func<KhachHang, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _context.KhachHangs.Where(filter).ToListAsync();
            }
            return await _context.KhachHangs.ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentAsync()
        {
            return await _context.Payments.ToListAsync();
        }


        public async Task<IEnumerable<Payment>> GetAllIdHDAsync(string id)
        {
            return await _context.Payments.Where(p => p.IdUser == id).ToListAsync();
        }

        public async Task<KhachHang> GetByEmailAsync(string id)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(p => p.GoogleAccount == id.Trim());
        }

        public async Task<KhachHang> GetByIdAsync(string id)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(p => p.IdUser == id.Trim());
        }

        public async Task<KhachHang> GetByAccountAsync(string id)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(p => p.IdUserNavigation.UserName == id.Trim());
        }
        public async Task UpdateAsync(KhachHang KhachHang)
        {
            try
            {
                _context.KhachHangs.Update(KhachHang);
                await _context.SaveChangesAsync();
            } catch (Exception ex) { throw new NotImplementedException();  }
            
        }

        public Task<IEnumerable<Payment>> GetAllHopDongAsync()
        {
            throw new NotImplementedException();
        }
    }
}
