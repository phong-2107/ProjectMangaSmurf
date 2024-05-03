using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IHopdongRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task AddAsync(Payment hd);
        string GenerateHD();
    }
}
