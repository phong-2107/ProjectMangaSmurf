using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ProjectDBContext _context;

        public EFUserRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
