using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IHopdongRepository
    {
        Task<IEnumerable<HopDong>> GetAllAsync();
    }
}
