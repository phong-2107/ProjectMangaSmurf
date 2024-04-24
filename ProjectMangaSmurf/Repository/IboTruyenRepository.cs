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
        Task<IEnumerable<BoTruyen>> GetRankingAsync();
        Task<IEnumerable<LoaiTruyen>> GetAllLoaiTruyens();
        Task<List<BoTruyen>> GetComicsByAuthorId(string authorId);
        string GenerateBoTruyenId();
        List<string> GetListLoaiAsync(string id);
        Task<BoTruyen> GetByIdAsync(string id);
        Task<BoTruyen> RandomAsync();
        Task<LoaiTruyen> GetLoaiByIdAsync(string id);
        Task<LoaiTruyen> GetLoaiByNameAsync(string name);
        Task AddAsync(BoTruyen botruyen);
        Task UpdateAsync(BoTruyen botruyen);
        Task DeleteAsync(string id);
        string getNameTGById(string id);
        Task<int> CountByComicTypeId(string comicTypeId);
        Task<int> CountByAuthorId(string comicAuthId);
    }
}
