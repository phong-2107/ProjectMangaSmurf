using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IboTruyenRepository
    {
        Task<IEnumerable<BoTruyen>> GetAllAsync();
        Task<IEnumerable<BoTruyen>> GetAllAllAsync();
        Task<IEnumerable<BoTruyen>> GetAllAsyncByChapterEarliest();
        Task<IEnumerable<BoTruyen>> GetAllTrangThaiAsync(int id);
        Task<IEnumerable<BoTruyen>> GetAllByTopic(string name);
        Task<IEnumerable<BoTruyen>> SearchByNameAsync(string name);
        Task<IEnumerable<BoTruyen>> GetRankingAsync();
        Task<IEnumerable<BoTruyen>> GetAllAsyncByFollow();
        Task<IEnumerable<BoTruyen>> GetAllAsyncByDay();
        Task<IEnumerable<BoTruyen>> GetAllAsyncByMonth();
        Task<IEnumerable<BoTruyen>> GetAllAsyncByRate();
        Task<IEnumerable<BoTruyen>> GetTrendingAsync();
        Task<IEnumerable<LoaiTruyen>> GetAllLoaiTruyens();
        Task<IEnumerable<Chapter>> GetListChapter(string id);
        Task<List<BoTruyen>> GetComicsByAuthorId(string authorId);
        Task DeleteAllChaptersAndDetails(string idBo);
        string GenerateBoTruyenId();
        List<string> GetListLoaiAsync(string id);
        Task<List<string>> GetListLoaiCAsync(string id);
        Task<List<string>> GetListLoaiQLAsync(string id);
        Task<BoTruyen> GetByIdAsync(string id);
        Task<BoTruyen> RandomAsync();
        Task<LoaiTruyen> GetLoaiByIdAsync(string id);
        Task<LoaiTruyen> GetLoaiByNameAsync(string name);
        Task AddAsync(BoTruyen botruyen);
        Task UpdateAsync(BoTruyen botruyen);
        Task DeleteAsync(string id);
        string getNameTGById(string id);
        Task<int> GetAllView();
        Task<int> CountChapterById(string id);
        Task<int> CountByComicTypeId(string comicTypeId);
        Task<int> CountByAuthorId(string comicAuthId);

        Task<IEnumerable<BoTruyen>> SearchComicsAsync(string query);
        Task<BoTruyen> GetBoTruyenWithMaxViewsAsync();
        Task<BoTruyen> GetBoTruyenWithMaxFollowsAsync();
    }
}
