using ProjectMangaSmurf.Models;
using System.Linq;

namespace ProjectMangaSmurf.Repository
{
    public interface IService
    {
        IQueryable<ServicePackConfig> GetQueryV();
        // Other method signatures
    }
}
