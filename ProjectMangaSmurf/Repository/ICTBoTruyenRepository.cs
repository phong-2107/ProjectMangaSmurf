using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface ICTBoTruyenRepository
    {
        Task AddAsync(CtBoTruyen botruyen);
        Task UpdateAsync(CtBoTruyen botruyen);
        Task DeleteAsync(string id);
        Task<CtBoTruyen> GetByIdAsync(string idkh, string idbo);
        Task<CtBoTruyen> GetByIdFollowAsync(string idkh, string idbo);
        Task<IEnumerable<CtBoTruyen>> GetAllAsyncByID(string id);
        Task<IEnumerable<CtBoTruyen>> GetAllAsyncFollowByID(string id);
        Task<IEnumerable<CtBoTruyen>> GetAllAsyncHistoryByID(string id);
    }
}
