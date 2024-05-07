using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFAvatarRepository : IAvatarRepository
    {
        private readonly ProjectDBContext _context;

        public EFAvatarRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Avatar>> GetAllAsync()
        {
            return await _context.Avatars.ToListAsync();
        }

        public async Task<Avatar> GetByIdAsync(string id)
        {
            return await _context.Avatars.FirstOrDefaultAsync(p => p.IdAvatar == id.Trim());
        }
        public async Task<Avatar> RandomAvatarAsync()
        {
            var activeAvatars = _context.Avatars.Where(a => a.Active == true).ToList();
            Random rand = new Random();
            int index = rand.Next(activeAvatars.Count);
            return await Task.FromResult(activeAvatars[index]);
        }
    }
}
