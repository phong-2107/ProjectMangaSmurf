using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IAvatarRepository
    {
        Task<Avatar> GetByIdAsync(string id);
        Task<Avatar> RandomAvatarAsync();
    }
}
