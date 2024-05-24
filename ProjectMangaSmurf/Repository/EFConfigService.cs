using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Repository;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Services
{
    public class EFConfigService : IConfigService
    {
        private readonly ProjectDBContext _context;

        public EFConfigService(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<string> GetConfigValueAsync(int id)
        {
            var config = await _context.WebMediaConfigs.FindAsync(id);
            return config?.ConfigValue;
        }
    }
}