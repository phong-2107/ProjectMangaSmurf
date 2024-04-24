using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectMangaSmurf.Areas.Identity.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IStaffRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<NhanVien> GetByIdAsync(string id);
        Task<NhanVien> GetAccountByIdAsync(string name, string pass);
        Task AddAsync(NhanVien Nv);
        Task UpdateAsync(NhanVien Nv);
        Task DeleteAsync(NhanVien id);
        IQueryable<NhanVien> GetQuery(); 
        string GenerateStaffId();
    }

}
