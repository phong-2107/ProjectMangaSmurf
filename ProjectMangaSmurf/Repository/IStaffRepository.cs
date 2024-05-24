using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IStaffRepository
    {
        Task<IEnumerable<User>> GetAllAsync();  // List all staff members
        Task<IEnumerable<NhanVien>> GetAllAsyncStaff();
        Task<User> GetAllStaffInfo(string id);
        Task<NhanVien> GetByIdAsync(string id);     // Get a single staff member by Id
        Task<User> GetAccountByIdAsync(string username, string password);  // Authenticate a staff member
        Task AddAsync(NhanVien nv);                 // Add a new staff member
        Task UpdateAsync(NhanVien nv, User uv);              // Update a staff member's details
        Task DeleteAsync(string id);                // Soft delete a staff member by Id
        IQueryable<NhanVien> GetQuery();            // Get a queryable interface to the staff members
        string GenerateStaffId();                   // Generate a new unique staff Id
        Task<NhanVien> GetByAccountAsyncStaff(string id);
        Task<PermissionsList> GetPermissionByIdAsync(byte id);
        Task UpdatePermissionAsync(PermissionsList permission);
        Task<IEnumerable<PermissionsList>> GetAllRBACAsync();

        Task<NhanVien> GetByEmailAsync(string id);
        Task<IEnumerable<StaffPermissionsDetail>> GetPermissionsByUserIdAsync(string userId);

        Task RemovePermissionAsync(StaffPermissionsDetail permission);

        Task AddPermissionAsync(StaffPermissionsDetail permissionDetail);

        Task UpdatePermissionDetailAsync(StaffPermissionsDetail permission);

        Task<User> GetStatsByIdAsync(String id);
        Task UpdateStatsAsync(User permission);
        Task<NhanVien> GetByIdSAsync(string id);
<<<<<<< HEAD
=======
        IQueryable<NhanVien> GetQueryV();
        Task UpdatePermissionDetail2Async(StaffPermissionsDetail permissionDetail);
        Task<StaffPermissionsDetail> GetPermissionDetailByIdAsync(string idUser, byte idPermissions);

        Task DeleteAllDetails(string id);
>>>>>>> 4fb584e170da6c6baae985c62871a8900f39bc3a
    }
}
