using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IAvatarRepository
    {
        Task<IEnumerable<Avatar>> GetAllAsync();
        Task<Avatar> GetByIdAsync(string id);
        Task<Avatar> RandomAvatarAsync();
    }
}
