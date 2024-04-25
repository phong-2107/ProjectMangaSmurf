using ProjectMangaSmurf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public interface IComicTypeRepository
    {
        Task<IEnumerable<LoaiTruyen>> GetAllAsync();
        Task AddAsync(LoaiTruyen type);
        Task UpdateAsync(LoaiTruyen type);
        Task DeleteAsync(string id);
        Task<LoaiTruyen> GetByIdAsync(string id);
        Task<LoaiTruyen> GetLoaiByIdAsync(string id);
        Task<LoaiTruyen> GetLoaiByNameAsync(string name);

        Task<List<string>> GetListLoaiAsync(string id);

        Task<string> GenerateLoaiTruyenId();
    }
}
