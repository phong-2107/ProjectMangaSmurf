using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public class EFboTruyenRepository : IboTruyenRepository
    {
        private readonly ProjectDBContext _context;

        public EFboTruyenRepository(ProjectDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BoTruyen>> GetAllAsync()
        {
            return await _context.BoTruyens.ToListAsync();
        }

        public async Task<BoTruyen> GetByIdAsync(string id)
        {
            return await _context.BoTruyens.Include(p => p.CtBoTruyens).FirstOrDefaultAsync(p => p.IdBo == id);
        }

        public async Task AddAsync(BoTruyen botruyen)
        {
            _context.BoTruyens.Add(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BoTruyen botruyen)
        {
            _context.BoTruyens.Update(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var botruyen = await _context.BoTruyens.FindAsync(id);
            _context.BoTruyens.Remove(botruyen);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BoTruyen>> GetAllByTopic(string name)
        {
            var getid = _context.LoaiTruyens.FirstOrDefault(p => p.TenLoai.Trim().Equals(name.Trim()));
            var getListTruyen = _context.CtLoaiTruyens.Where(p => p.IdLoai.Trim().Equals(getid.IdLoai)).ToList();
            List<BoTruyen> list = new List<BoTruyen>();
            foreach (var item in getListTruyen)
            {
                var find = _context.BoTruyens.FirstOrDefault(p => p.IdBo == item.IdBo);
                if(find != null)
                {
                    list.Add(find);
                }
            }
            IEnumerable<BoTruyen> listbo = list;
            return await Task.FromResult(listbo);
        }



        public async Task<LoaiTruyen> GetLoaiByIdAsync(string id)
        {
            return await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.IdLoai == id);
        }

        public async Task<LoaiTruyen> GetLoaiByNameAsync(string name)
        {
            return await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.TenLoai.Trim().Equals(name.Trim()));
        }

        public async Task<BoTruyen> RandomAsync()
        {
            var list =  _context.BoTruyens.ToList();
            Random random = new Random();
            int randomIndex = random.Next(0, list.Count);
            var randomElement = list[randomIndex];
            return await Task.FromResult(randomElement);
        }

        public async Task<IEnumerable<BoTruyen>> GetRankingAsync()
        {
            return await _context.BoTruyens.OrderByDescending(p => p.TongLuotXem).Take(10).ToListAsync();
        }



        public List<string> GetListLoaiAsync(string id)
        {
            var list = _context.CtLoaiTruyens.Where(p => p.IdBo == id.Trim()).ToList();
            List<string> listLoai = new List<string>();
            if(list.Count > 0)
            {
                foreach(var item in list)
                {
                    var loai = _context.LoaiTruyens.FirstOrDefault(p => p.IdLoai == item.IdLoai);
                    listLoai.Add(loai.TenLoai);
                }
            }
            return listLoai;
        }

        public string getNameTGById(string id)
        {
            var Tg = _context.TacGia.FirstOrDefault(p => p.IdTg == id);
            return Tg.TenTg;
        }

        public async Task<IEnumerable<BoTruyen>> GetAllTrangThaiAsync(int id)
        {
            var list = _context.BoTruyens.ToList();
            List<BoTruyen> listTTActive =  new List<BoTruyen>();
            if (id == 1)
            {
                listTTActive = list.Where(p => p.TrangThai == id).ToList();
            }
            else if (id == 0)
            {
                listTTActive = list.Where(p => p.TrangThai == id).ToList();
            }
            return await Task.FromResult<IEnumerable<BoTruyen>>(listTTActive);
        }

        public async Task<IEnumerable<BoTruyen>> GetAllAsyncByChapterEarliest()
        {
            var danhSachBoTruyen = await _context.BoTruyens
                                        .OrderByDescending(boTruyen => boTruyen.Chapters.Max(chap => chap.ThoiGian))
                                        .ToListAsync();

            return danhSachBoTruyen;
        }


        public async Task<IEnumerable<LoaiTruyen>> GetAllLoaiTruyens()
        {
            var loaitruyen = await _context.LoaiTruyens.ToListAsync();
            return loaitruyen;
        }

        public string GenerateBoTruyenId()
        {
            string idPrefix = "BT";
            int idLength = 8;
            string maxId = _context.BoTruyens.Select(kh => kh.IdBo)
                                               .OrderByDescending(id => id)
                                               .FirstOrDefault();
            if (maxId == null)
            {
                return idPrefix + "00000001";
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
