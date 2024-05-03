using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User id);
    }
}
