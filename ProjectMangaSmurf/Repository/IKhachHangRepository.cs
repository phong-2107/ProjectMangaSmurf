using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang> GetByIdAsync(string id);

        Task<KhachHang> GetAccountByIdAsync(string name, string pass);

    }
}
