using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public interface IWebMediaRepository
    {
        Task<IEnumerable<WebMediaConfig>> GetAllConfigsAsync();
        Task<WebMediaConfig> GetConfigByIdAsync(int id);
        Task UpdateConfigAsync(WebMediaConfig config);
        Task<WebMediaConfig> GetConfigByIdAsync2(int id);
    }

}
