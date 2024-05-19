using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Repository;
using ProjectMangaSmurf.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models.ViewModels;
using ProjectMangaSmurf.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ProjectMangaSmurf.Areas.Common.Repository;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class StaffManager : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;
        private readonly ProjectDBContext _context;
        public StaffManager(IStaffRepository staffRepository, IUserRepository userRepository, ProjectDBContext db)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _context = db;
        }

        public async Task<IActionResult> RBAC()
        {
            IEnumerable<PermissionsList> permissionsList = await _staffRepository.GetAllRBACAsync() ?? Enumerable.Empty<PermissionsList>();
            return View(permissionsList);
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<NhanVien> query = _staffRepository.GetQueryV(); 

            var list = await query.ToListAsync(); 
            return View(list); 
        }

        public async Task<IActionResult> SelfDetail(string id)
        {
            var staff = await _staffRepository.GetByIdSAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var staffPermissions = await _staffRepository.GetPermissionsByUserIdAsync(id);
            var allPermissions = await _staffRepository.GetAllRBACAsync();

            ViewBag.Pn = staffPermissions;
            ViewBag.AllPermissions = allPermissions;

            var viewModel = new StaffDetailsViewModel
            {
                Staff = staff
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Detail(string id)
        {
            var staff = await _staffRepository.GetByIdSAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var staffPermissions = await _staffRepository.GetPermissionsByUserIdAsync(id);
            var allPermissions = await _staffRepository.GetAllRBACAsync();

            ViewBag.Pn = staffPermissions;
            ViewBag.AllPermissions = allPermissions;

            var viewModel = new StaffDetailsViewModel
            {
                Staff = staff
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> TogglePermissionStatus(string idUser, byte idPermissions)
        {
            var permission = await _staffRepository.GetPermissionDetailByIdAsync(idUser, idPermissions);
            if (permission == null)
            {
                return NotFound("Permission not found.");
            }

            permission.Active = !(permission.Active ?? false);
            await _staffRepository.UpdatePermissionDetailAsync(permission);

            return RedirectToAction("Detail", new { id = idUser });
        }


        public async Task<IActionResult> Update(string id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        [HttpPost]
        private async Task UpdatePermissionStatus(byte id, byte status)
        {
            var permission = await _staffRepository.GetPermissionByIdAsync(id);
            if (permission != null)
            {
                permission.PermissionsStats = status;
                await _staffRepository.UpdatePermissionAsync(permission);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStaffStatus(string IdUser, bool Active)
        {
            var user = await _userRepository.GetByIdAsync(IdUser);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Active = Active;
            await _userRepository.UpdateAsync(user);
            return RedirectToAction("Detail", new { id = IdUser });
        }


        [HttpPost]
        public async Task<IActionResult> SetActive(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)1);  // Active status
            return RedirectToAction("RBAC");
        }

        [HttpPost]
        public async Task<IActionResult> SetInactive(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)0);  // Inactive status
            return RedirectToAction("RBAC");
        }

        [HttpPost]
        public async Task<IActionResult> SetMaintenance(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)2);  // Maintenance status
            return RedirectToAction("RBAC");
        }

        [HttpPost]
        public async Task<IActionResult> SetStop(int id)
        {
            await UpdatePermissionStatus((byte)id, (byte)3);  // Stop status
            return RedirectToAction("RBAC");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Taikhoan, string Email, string matKhau)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existingUser = await _staffRepository.GetByEmailAsync(Email);
            if (existingUser != null)
            {
                ViewBag.Error = "This email already exists"; 
                return View();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    User user = new User
                    {
                        IdUser = _staffRepository.GenerateStaffId(),
                        UserName = Taikhoan.Trim(),
                        Password = PasswordHasher.HashPassword(matKhau),
                        Email = Email.Trim(),
                        TimeCreated = DateTime.Now,
                        TimeUpdated = DateTime.Now,
                        UserRole = false,
                        Active = true
                    };

                    await _userRepository.AddAsync(user);
                    NhanVien nhanVien = new NhanVien
                    {
                        IdUser = user.IdUser,
                        StaffRole = true,
                    };

                    await _staffRepository.AddAsync(nhanVien);

                    // Check if the user is an admin
                    if (nhanVien.StaffRole == true)
                    {
                        // Assign all permissions (1-73) to the admin user
                        for (byte i = 1; i <= 73; i++)
                        {
                            var staffPermissionDetail = new StaffPermissionsDetail
                            {
                                IdUser = user.IdUser,
                                IdPermissions = i,
                                Active = true
                            };
                            await _staffRepository.AddPermissionAsync(staffPermissionDetail);
                        }
                    }

                    transaction.Commit();

                    // Redirect to RegisterRBAC with the IdUser of the newly created user
                    return RedirectToAction("UpdateAndRBAC", "StaffManager", new { id = user.IdUser });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error: " + ex.Message;
                    return View();
                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateAndRBAC(string id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound("Staff not found.");
            }

            var user = await _userRepository.GetByIdAsync(staff.IdUser);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var permissionsList = await _staffRepository.GetAllRBACAsync() ?? new List<PermissionsList>();
            var existingPermissions = await _staffRepository.GetPermissionsByUserIdAsync(user.IdUser) ?? new List<StaffPermissionsDetail>();

            ViewBag.IdUser = user.IdUser;
            ViewBag.Username = user.UserName;
            ViewBag.FullName = user.FullName;
            ViewBag.BirthDate = user.Birth;
            ViewBag.Gender = user.Gender;
            ViewBag.StaffRole = staff.StaffRole;
            ViewBag.Active = user.Active;
            ViewBag.Phone = user.Phone;
            ViewBag.AvailablePermissions = permissionsList;
            ViewBag.SelectedPermissions = existingPermissions.Where(p => p.Active ?? false).Select(p => p.IdPermissions).ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAndRBAC(string IdUser, string Username, string FullName, DateOnly? BirthDate, byte? Gender, bool StaffRole, List<byte> SelectedPermissions)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userRepository.GetByIdAsync(IdUser);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update user details
                    user.UserName = Username.Trim();
                    user.FullName = FullName.Trim();
                    user.Birth = BirthDate;
                    user.Gender = Gender ?? default(byte);
                    user.TimeUpdated = DateTime.Now;

                    await _userRepository.UpdateAsync(user);

                    // Update staff role
                    var staff = await _staffRepository.GetByIdAsync(IdUser);
                    staff.StaffRole = StaffRole;
                    await _staffRepository.UpdateAsync(staff, user);

                    // Update permissions
                    var existingPermissions = await _staffRepository.GetPermissionsByUserIdAsync(user.IdUser);

                    foreach (var permission in existingPermissions)
                    {
                        permission.Active = SelectedPermissions.Contains(permission.IdPermissions);
                        await _staffRepository.UpdatePermissionDetailAsync(permission);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return RedirectToAction("Index", "StaffManager");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                    return View();
                }
            }
        }
    }
}
