using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProjectMangaSmurf.Repository
{
    public class EFStaffRepository : IStaffRepository
    {
        private readonly ProjectDBContext _context;
        private readonly ILogger<EFStaffRepository> _logger;

        public EFStaffRepository(ProjectDBContext context, ILogger<EFStaffRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<PermissionsList> GetPermissionByIdAsync(byte id)
        {
            return await _context.PermissionsLists.FindAsync(id);
        }

        public async Task UpdatePermissionAsync(PermissionsList permission)
        {
            _context.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionDetailAsync(StaffPermissionsDetail permission )
        {
            _context.Update(permission);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePermissionDetail2Async(StaffPermissionsDetail permissionDetail)
        {
            _context.StaffPermissionsDetails.Update(permissionDetail);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Filter NhanViens where UserRole indicates they are staff
            return await _context.Users.Where(nv => nv.UserRole == false).ToListAsync();
        }
        public async Task<IEnumerable<NhanVien>> GetAllAsyncStaff()
        {
            return await _context.NhanViens.ToListAsync();
        }

        public async Task<User> GetAllStaffInfo(string id)
        {
            var staffList = await GetAllAsyncStaff();
            var user = await _context.Users.FirstOrDefaultAsync(nv => nv.IdUser == id);
            return user;
        }

        public async Task<IEnumerable<StaffPermissionsDetail>> GetPermissionsByUserIdAsync(string userId)
        {
            return await _context.StaffPermissionsDetails
                .Where(x => x.IdUser == userId)
                .ToListAsync();
        }

        public async Task<NhanVien> GetByEmailAsync(string id)
        {
            return await _context.NhanViens.FirstOrDefaultAsync(p => p.IdUserNavigation.Email == id.Trim());
        }
        public async Task<IEnumerable<PermissionsList>> GetAllRBACAsync()
        {
            return await _context.PermissionsLists.ToListAsync();
        }

        public async Task<NhanVien> GetByIdAsync(string id)
        {
            return await _context.NhanViens.FirstOrDefaultAsync(nv => nv.IdUser == id);
        }

        public async Task<NhanVien> GetByIdSAsync(string id)
        {
            return await _context.NhanViens
                .Include(nv => nv.IdUserNavigation)
                .FirstOrDefaultAsync(nv => nv.IdUser == id);
        }

        public async Task<User> GetAccountByIdAsync(string username, string password)
        {
            // Include UserRole check to ensure correct role
            return await _context.Users
                                 .FirstOrDefaultAsync(nv => nv.UserName == username &&
                                                            nv.Password == password &&
                                                            nv.UserRole == false);
        }
        public async Task<NhanVien> GetByAccountAsyncStaff(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(p => p.UserName == id.Trim() || p.Email == id.Trim());

            if (user == null)
            {
                return null;
            }

            return await _context.NhanViens.FirstOrDefaultAsync(p => p.IdUser == user.IdUser);
        }
        public async Task AddAsync(NhanVien nv)
        {
            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVien nv , User uv )
        {
            _context.NhanViens.Update(nv);
            _context.Users.Update(uv);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var nv = await _context.Users.FindAsync(id);
            if (nv != null)
            {
                nv.Active = false;  // Assume IsActive is a soft-delete flag
                _context.Update(nv);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<NhanVien> GetQuery()
        {
            return _context.NhanViens;
        }

        public IQueryable<NhanVien> GetQueryV()
        {
            return _context.NhanViens.Include(kh => kh.IdUserNavigation);
        }
        public string GenerateStaffId()
        {
            try
            {
                string idPrefix = "ID";
                int idLength = 8;
                var maxId = _context.Users.Select(nv => nv.IdUser)
                                              .Where(id => id.StartsWith(idPrefix))
                                              .OrderByDescending(id => id)
                                              .FirstOrDefault();

                if (maxId == null)
                {
                    return idPrefix + "00000001"; // Starting ID
                }

                if (int.TryParse(maxId.Substring(idPrefix.Length), out var numericId))
                {
                    numericId++;
                    return idPrefix + numericId.ToString().PadLeft(idLength, '0');
                }

                throw new InvalidOperationException("Failed to parse the numeric part of the staff ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating new staff ID");
                throw;
            }
        }

        public async Task RemovePermissionAsync(StaffPermissionsDetail permission)
        {
            _context.StaffPermissionsDetails.Remove(permission);
            await _context.SaveChangesAsync();
        }

        public async Task AddPermissionAsync(StaffPermissionsDetail permissionDetail)
        {
            _context.StaffPermissionsDetails.Add(permissionDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetStatsByIdAsync(string userId) // Correct return type
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
        }

        public async Task UpdateStatsAsync(User permission)
        {
            _context.Users.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<StaffPermissionsDetail> GetPermissionDetailByIdAsync(string idUser, byte idPermissions)
        {
            return await _context.StaffPermissionsDetails
                .FirstOrDefaultAsync(p => p.IdUser == idUser && p.IdPermissions == idPermissions);
        }

    }
   
}


