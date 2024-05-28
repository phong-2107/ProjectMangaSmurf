using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IHopdongRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();

        Task<IEnumerable<Payment>> GetPaymentByIdAsync(string id);
        Task AddAsync(Payment hd);
        Task<Payment> GetByIdAsync(string id);
        string GenerateHD();

        Task<ServicePackConfig> GetPackByIdAsync(string id);

        ServicePackConfig GetPackById(String id);

    }
}
