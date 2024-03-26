using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IboTruyenRepository
    {
        Task<IEnumerable<BoTruyen>> GetAllAsync();
        Task<IEnumerable<BoTruyen>> GetAllByTopic(string name);
        Task<IEnumerable<BoTruyen>> GetRankingAsync();

        List<string> GetListLoaiAsync(string id);

        Task<BoTruyen> GetByIdAsync(string id);
        Task<BoTruyen> RandomAsync();
        Task<LoaiTruyen> GetLoaiByIdAsync(string id);
        Task AddAsync(BoTruyen botruyen);
        Task UpdateAsync(BoTruyen botruyen);
        Task DeleteAsync(string id);


    }
}
