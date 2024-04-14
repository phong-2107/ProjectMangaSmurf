using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface ILoaiTruyenRepository
    {
        Task<IEnumerable<LoaiTruyen>> GetAllAsync();

        Task AddAsyncLoai(LoaiTruyen loai);
        Task AddAsyncCTLoai(CtLoaiTruyen ctloai);
    }
}
