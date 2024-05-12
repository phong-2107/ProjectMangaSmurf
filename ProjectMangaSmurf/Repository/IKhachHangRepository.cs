using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync(Expression<Func<KhachHang, bool>> filter = null);
        Task<IEnumerable<Payment>> GetAllIdHDAsync(string id);
        Task<IEnumerable<Payment>> GetAllHopDongAsync();
        Task<KhachHang> GetByIdAsync(string id);
        Task<KhachHang> GetByAccountAsync(string id);
        Task<KhachHang> GetByEmailAsync(string id);
        Task<KhachHang> GetAccountByIdAsync(string name, string pass);
        Task AddAsync(KhachHang KhachHang);
        Task UpdateAsync(KhachHang KhachHang);
        Task DeleteAsync(KhachHang id);
        Task<IEnumerable<Payment>> GetAllPaymentAsync();
        IQueryable<KhachHang> GetQuery(); 
        string GenerateCustomerId();
    }

}
