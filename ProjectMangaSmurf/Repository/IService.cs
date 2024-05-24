using ProjectMangaSmurf.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public interface IService
    {
        IQueryable<ServicePackConfig> GetQueryV();
        Task<ServicePackConfig> GetByIdAsync(string id);
        Task UpdateAsync(ServicePackConfig servicePackConfig);
        Task DeleteAsync(string id);
        Task AddAsync(ServicePackConfig servicePackConfig);
        Task<string> GenerateId();
    }
}
