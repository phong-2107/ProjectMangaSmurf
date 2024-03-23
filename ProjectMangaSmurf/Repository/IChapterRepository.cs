using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IChapterRepository
    {
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<IEnumerable<Chapter>> GetAllByIdAsync(string id);
        Task<IEnumerable<CtChapter>> GetAllCTByIdAsync(string id, int stt);
        Task<Chapter> GetByIdAsync(string id, int stt);
        Task AddAsync(Chapter botruyen);
        Task UpdateAsync(Chapter botruyen);
        Task DeleteAsync(string id);
    }
}
