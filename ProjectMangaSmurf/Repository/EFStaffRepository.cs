using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Areas.Identity.Data;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Linq.Expressions;

namespace ProjectMangaSmurf.Repository
{
    public class EFStaffRepository : IStaffRepository
    {
        private readonly ProjectDBContext _context;

        public EFStaffRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(NhanVien Nv)
        {
            try
            {
                _context.NhanViens.Add(Nv);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new NotImplementedException(); }
        }
        public IQueryable<NhanVien> GetQuery()
        {
            return _context.NhanViens;
        }
        public async Task DeleteAsync(NhanVien id)
        {
            try
            {
                var n = await _context.KhachHangs.FindAsync(id);
                n.Active = false;
                _context.NhanViens.Update(id);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new NotImplementedException(); }
        }

        public string GenerateStaffId()
        {
            string idPrefix = "S";
            int idLength = 8;
            string maxId = _context.NhanViens.Select(kh => kh.IdNv)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "01";
            }
            else
            {
                int currentNumber = int.Parse(maxId.Substring(idPrefix.Length));
                string newId = idPrefix + (currentNumber + 1).ToString().PadLeft(idLength, '0');
                return newId;
            }
        }
        public Task<NhanVien> GetAccountByIdAsync(string name, string pass)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.applicationUsers.Where(p => p.LockoutEnabled == true).ToListAsync();
        }

        public async Task<NhanVien> GetByIdAsync(string id)
        {
            return await _context.NhanViens.FirstOrDefaultAsync(p => p.IdNv == id.Trim());
        }
        public async Task UpdateAsync(NhanVien Nv)
        {
            try
            {
                _context.NhanViens.Update(Nv);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw new NotImplementedException(); }

        }
    }
}
