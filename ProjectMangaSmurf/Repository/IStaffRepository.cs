using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IStaffRepository
    {
        Task<IEnumerable<NhanVien>> GetAllAsync(Expression<Func<NhanVien, bool>> filter = null);
        Task<NhanVien> GetByIdAsync(string id);
        Task<NhanVien> GetAccountByIdAsync(string name, string pass);
        Task AddAsync(NhanVien Nv);
        Task UpdateAsync(NhanVien Nv);
        Task DeleteAsync(NhanVien id);
        IQueryable<NhanVien> GetQuery(); 
        string GenerateStaffId();
    }

}
