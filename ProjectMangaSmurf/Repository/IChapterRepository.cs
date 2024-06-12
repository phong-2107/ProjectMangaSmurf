using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IChapterRepository
    {
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<IEnumerable<Chapter>> GetAllByIdAsync(string id);
        Task<IEnumerable<CtChapter>> GetAllCTByIdAsync(string id, int stt);
        Task<Chapter> GetByIdAsync(string id, int stt);
        Task<CtHoatDong> GetCTHDAsync(string id, string kh, int stt);
        Task<List<Chapter>> GetChaptersByComicId(string comicId);
        Task<CtChapter> GetPageByIdAsync(string idBo, int sttChap, int soTrang);
        Task UpdatePageNumberAsync(string idBo, int sttChap, int currentSoTrang, int newSoTrang);
        Task DeletePageAsync(string idBo, int sttChap, int soTrang);
        int GetEarliestByIdAsync(string id);
        byte GetCostByIdAsync(string id, int stt);
        string CalculateTimeAgo(DateTime date);
        Chapter GetChapterEarliestByIdAsync(string id);
        Task AddAsync(Chapter chapter);
        Task AddAsyncCTHD(CtHoatDong ct);
        Task<CtChapter> GetCTChapterByIdAsync(string idBo, int sttChap, int soTrang);
        Task UpdateCTChapterAsync(CtChapter ctChapter);
        Task AddAsyncCT(CtChapter CTs);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(string id);
        Task<int> GetMaxChapterNumberAsync(string idBo);
        Task<int> SaveAsync();
        Task ToggleActiveStatus(string idBo, int sttChap, int soTrang);
        Task ReplaceImageAsync(string idBo, int sttChap, int soTrang, IFormFile newImage);
        Task DeletePageAndUpdateSubsequentAsync(string idBo, int sttChap, int soTrang);
        Task<int> GetMaxPageNumber(string idBo, int sttChap);
        Task DeleteCTChapterAsync(CtChapter ctChapter);
        Task DeleteChapterAsync(Chapter chapter);
        // In IChapterRepository interface
        Task<IEnumerable<CtHoatDong>> GetAllCtHoatDongByIdAsync(string idBo, int sttChap);
        // In IChapterRepository interface
        Task DeleteCtHoatDongAsync(CtHoatDong ctHoatDong);

        Task<int> GetMaxSttChapAsync(string idBo);
    }
}
