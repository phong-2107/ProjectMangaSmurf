using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IChapterRepository
    {
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<IEnumerable<Chapter>> GetAllByIdAsync(string id);
        Task<IEnumerable<CtChapter>> GetAllCTByIdAsync(string id, int stt);
        Task<Chapter> GetByIdAsync(string id, int stt);
        Task<List<Chapter>> GetChaptersByComicId(string comicId);
        Task<CtChapter> GetPageByIdAsync(string idBo, int sttChap, int soTrang);
        Task UpdatePageNumberAsync(string idBo, int sttChap, int currentSoTrang, int newSoTrang);
        Task DeletePageAsync(string idBo, int sttChap, int soTrang);
        int GetEarliestByIdAsync(string id);
        string CalculateTimeAgo(DateTime date);
        Chapter GetChapterEarliestByIdAsync(string id);
        Task AddAsync(Chapter chapter);
        Task AddAsyncCT(CtChapter CTs);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(string id);
        Task<int> GetMaxChapterNumberAsync(string idBo);
        Task<int> SaveAsync();
        Task ToggleActiveStatus(string idBo, int sttChap, int soTrang);
        Task ReplaceImageAsync(string idBo, int sttChap, int soTrang, IFormFile newImage);
        Task DeletePageAndUpdateSubsequentAsync(string idBo, int sttChap, int soTrang);
        Task<int> GetMaxPageNumber(string idBo, int sttChap);
    }
}
