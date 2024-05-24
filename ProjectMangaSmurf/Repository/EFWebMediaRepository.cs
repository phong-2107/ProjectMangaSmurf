using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public class EFWebMediaRepository : IWebMediaRepository
    {
        private readonly ProjectDBContext _context;

        public EFWebMediaRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WebMediaConfig>> GetAllConfigsAsync()
        {
            return await _context.WebMediaConfigs.ToListAsync();
        }

        public async Task<WebMediaConfig> GetConfigByIdAsync(int id)
        {
            return await _context.WebMediaConfigs.FindAsync(id);
        }

        public async Task<WebMediaConfig> GetConfigByIdAsync2(int id)
        {
            return await _context.WebMediaConfigs
                                 .Where(config => config.IdConfig == id && config.Active == true)
                                 .FirstOrDefaultAsync();
        }

        public async Task UpdateConfigAsync(WebMediaConfig config)
        {
            _context.WebMediaConfigs.Update(config);
            await _context.SaveChangesAsync();
        }
    }
}
