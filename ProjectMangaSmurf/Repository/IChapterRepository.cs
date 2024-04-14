using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IChapterRepository
    {
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<IEnumerable<Chapter>> GetAllByIdAsync(string id);
        Task<IEnumerable<CtChapter>> GetAllCTByIdAsync(string id, int stt);
        Task<Chapter> GetByIdAsync(string id, int stt);
        int GetEarliestByIdAsync(string id);
        string CalculateTimeAgo(DateTime date);
        Chapter GetChapterEarliestByIdAsync(string id);
        Task AddAsync(Chapter chapter);
        Task AddAsyncCT(CtChapter CTs);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(string id);
    }
}
