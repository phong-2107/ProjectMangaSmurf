using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFFooterRepository : IFooterRepository
    {
        private readonly ProjectDBContext _context;

        public EFFooterRepository(ProjectDBContext context)
        {
            _context = context;
        }

        Footer IFooterRepository.GetFirstAsync()
        {
            return _context.Footers.FirstOrDefault();
        }
    }
}
