using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task AddAsyncKH(User user, KhachHang kh);
        Task AddAsyncNV(User user, NhanVien nv);
        Task UpdateAsync(User user);
        Task DeleteAsync(User id);

        Task<User> GetByIdAsync(string id);

        Task<User> GetByEmailAsync(string email);
    }
}
