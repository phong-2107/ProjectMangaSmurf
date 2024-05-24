using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ProjectMangaSmurf.Repository
{
    public class EFService : IService
    {
        private readonly ProjectDBContext _context;
        private readonly ILogger<EFService> _logger;

        public EFService(ProjectDBContext context, ILogger<EFService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IQueryable<ServicePackConfig> GetQueryV()
        {
            var servicePackConfigs = _context.ServicePackConfigs
                .Select(spc => new ServicePackConfig
                {
                    IdPack = spc.IdPack,
                    ParentPack = spc.ParentPack,
                    PackName = spc.PackName,
                    TicketSalary = spc.TicketSalary,
                    Amount = spc.Amount,
                    Discount = spc.Discount,
                    Description = spc.Description,
                    Active = spc.Active,
                    PurchaseCount = _context.Payments.Count(p => p.IdPack == spc.IdPack) // Calculate purchase count
                });

            return servicePackConfigs;
        }

        public async Task<ServicePackConfig> GetByIdAsync(string id)
        {
            return await _context.ServicePackConfigs.FindAsync(id);
        }

        public async Task UpdateAsync(ServicePackConfig servicePackConfig)
        {
            _context.Entry(servicePackConfig).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var servicePackConfig = await _context.ServicePackConfigs.FindAsync(id);
            if (servicePackConfig != null)
            {
                _context.ServicePackConfigs.Remove(servicePackConfig);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAsync(ServicePackConfig servicePackConfig)
        {
            _context.ServicePackConfigs.Add(servicePackConfig);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateId()
        {
            try
            {
                string idPrefix = "P";
                int idLength = 3;
                var maxId = await _context.ServicePackConfigs
                    .Where(p => p.IdPack.StartsWith(idPrefix))
                    .OrderByDescending(p => p.IdPack)
                    .Select(p => p.IdPack)
                    .FirstOrDefaultAsync();

                if (maxId == null)
                {
                    return idPrefix + "004"; // Starting ID
                }

                if (int.TryParse(maxId.Substring(idPrefix.Length), out var numericId))
                {
                    numericId++;
                    return idPrefix + numericId.ToString().PadLeft(idLength, '0');
                }

                throw new InvalidOperationException("Failed to parse the numeric part of the service pack ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating new service pack ID");
                throw;
            }
        }
    }
}
