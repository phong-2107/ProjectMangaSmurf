using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IboTruyenRepository
    {
        Task<IEnumerable<BoTruyen>> GetAllAsync();
        Task<IEnumerable<BoTruyen>> GetAllByTopic(string name);

        Task<BoTruyen> GetByIdAsync(string id);
        Task<LoaiTruyen> GetLoaiByIdAsync(string id);
        Task AddAsync(BoTruyen botruyen);
        Task UpdateAsync(BoTruyen botruyen);
        Task DeleteAsync(string id);
    }
}
