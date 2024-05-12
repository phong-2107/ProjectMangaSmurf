
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFWebMediaRepository : IWebMediaRepository
    {
        private readonly ProjectDBContext _context;

        public EFWebMediaRepository(ProjectDBContext context)
        {
            _context = context;
        }

        WebMediaConfig IWebMediaRepository.GetFirstAsync()
        {
            return _context.WebMediaConfigs.FirstOrDefault();
        }
    }
}
