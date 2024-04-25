using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public class EFComicTypeRepository : IComicTypeRepository
    {
        private readonly ProjectDBContext _context;

        public EFComicTypeRepository(ProjectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiTruyen>> GetAllAsync()
        {
            return await _context.LoaiTruyens.ToListAsync();
        }

        public async Task AddAsync(LoaiTruyen type)
        {
            _context.LoaiTruyens.Add(type);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiTruyen type)
        {
            _context.LoaiTruyens.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var type = await _context.LoaiTruyens.FindAsync(id);
            if (type != null)
            {
                _context.LoaiTruyens.Remove(type);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<LoaiTruyen> GetByIdAsync(string id)
        {
            return await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.IdLoai == id);
        }

        public async Task<LoaiTruyen> GetLoaiByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<LoaiTruyen> GetLoaiByNameAsync(string name)
        {
            return await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.TenLoai.Trim().Equals(name.Trim()));
        }

        public async Task<List<string>> GetListLoaiAsync(string id)
        {
            var list = _context.CtLoaiTruyens.Where(p => p.IdBo == id.Trim()).ToList();
            List<string> listLoai = new List<string>();
            foreach (var item in list)
            {
                var loai = await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.IdLoai == item.IdLoai);
                if (loai != null)
                {
                    listLoai.Add(loai.TenLoai);
                }
            }
            return listLoai;
        }

        public async Task<List<BoTruyen>> GetAllBoTruyensAsync()
        {
            return await _context.BoTruyens.ToListAsync();
        }
        public async Task<string> GenerateLoaiTruyenId()
        {
            string idPrefix = "LT";
            int idLength = 3;
            string maxId = _context.LoaiTruyens.Select(lt => lt.IdLoai)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "001";
            }
            else
            {
                int currentNumber = int.Parse(maxId.Substring(idPrefix.Length));
                string newId = idPrefix + (currentNumber + 1).ToString().PadLeft(idLength, '0');
                return newId;
            }
        }
    }
}
