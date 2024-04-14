using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface ITacGiaRepository
    {
        Task<IEnumerable<TacGium>> GetAllAsync();
    }
}
