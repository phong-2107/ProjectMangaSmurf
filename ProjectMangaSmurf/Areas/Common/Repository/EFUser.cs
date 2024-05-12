using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;

namespace ProjectMangaSmurf.Areas.Common.Repository
{

        public class EFUser : IUser
        {
            private ProjectDBContext _context;

            public EFUser(ProjectDBContext context)
            {
                _context = context;
            }

            public User FindByUsername(string username)
            {
                return _context.Users.FirstOrDefault(u => u.UserName == username);
            }

            public void AddUser(User user)
            {
                _context.Users.Add(user);
            }

            public void UpdateUser(User user)
            {
                _context.Entry(user).State = EntityState.Modified;
            }

            public void DeleteUser(string userId)
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }
            }

            public int SaveChanges()
            {
                return _context.SaveChanges();
            }
        

    }

}
