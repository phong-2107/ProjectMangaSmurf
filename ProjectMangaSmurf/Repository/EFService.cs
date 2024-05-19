using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Linq;

namespace ProjectMangaSmurf.Repository
{
    public class EFService : IService
    {
        private readonly ProjectDBContext _context;

        public EFService(ProjectDBContext context)
        {
            _context = context;
        }

        public IQueryable<ServicePackConfig> GetQueryV()
        {
            return _context.ServicePackConfigs.AsQueryable();
        }

        // Other method implementations
    }
}
