using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Areas.Common.Repository
{
    public interface IUser
    {
        User FindByUsername(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string userId);
        int SaveChanges();
    }

}
