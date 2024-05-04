using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ProjectDBContext _context;

        public EFUserRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsyncKH(User user, KhachHang kh)
        {
            _context.Users.Add(user);
            _context.KhachHangs.Add(kh);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsyncNV(User user, NhanVien nv)
        {
            _context.Users.Add(user);
            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.IdUser == id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
