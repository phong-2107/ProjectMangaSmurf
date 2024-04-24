using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IHopdongRepository
    {
        Task<IEnumerable<HopDong>> GetAllAsync();
        Task AddAsync(HopDong hd);
        string GenerateHD();
    }
}
