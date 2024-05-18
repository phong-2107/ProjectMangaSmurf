using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using System;

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
            return await _context.BoTruyens.Where(p => p.Active == true).ToListAsync();
        }
        public async Task<IEnumerable<BoTruyen>> GetAllAllAsync()
        {
            return await _context.BoTruyens.ToListAsync();
        }

        public async Task<BoTruyen> GetByIdAsync(string id)
        {
            return await _context.BoTruyens.FirstOrDefaultAsync(p => p.IdBo == id);
        }
        public async Task DeleteAllChaptersAndDetails(string idBo)
        {
            var comicl = await _context.BoTruyens
                .Include(lh => lh.CtLoaiTruyens)
                        .FirstOrDefaultAsync(c => c.IdBo == idBo);
            var comic = await _context.BoTruyens
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.CtChapters)
                        .FirstOrDefaultAsync(c => c.IdBo == idBo);

            if (comicl != null)
            {
                foreach (var chapter in comicl.CtLoaiTruyens)
                {
                    _context.CtLoaiTruyens.Remove(chapter);
                }
            }

            if (comic != null)
            {
                foreach (var chapter in comic.Chapters)
                {
                    foreach (var detail in chapter.CtChapters)
                    {
                        _context.CtChapters.Remove(detail);
                    }
                    _context.Chapters.Remove(chapter);
                }
                _context.BoTruyens.Remove(comic);
                await _context.SaveChangesAsync();
            }
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
            var author = await _context.BoTruyens.FindAsync(id);
            if (author != null)
            {
                _context.BoTruyens.Remove(author);
                await _context.SaveChangesAsync();
            }
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

        public async Task<WebMediaConfig> GetFirstAsync()
        {
            return await _context.WebMediaConfigs.FirstOrDefaultAsync();
        }


        public async Task<List<string>> GetListLoaiQLAsync(string id)
        {
            // Asynchronously execute the query and materialize results
            var list = await _context.CtLoaiTruyens
                                     .Where(p => p.IdBo == id.Trim())
                                     .ToListAsync();

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

        public async Task<List<string>> GetListLoaiCAsync(string id)
        {
            var list = await _context.CtLoaiTruyens.Where(p => p.IdBo == id.Trim()).ToListAsync();
            List<string> listLoai = new List<string>();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var loai = await _context.LoaiTruyens.FirstOrDefaultAsync(p => p.IdLoai == item.IdLoai);
                    if (loai != null)
                    {
                        listLoai.Add(loai.TenLoai);
                    }
                }
            }
            return listLoai;
        }

        public List<string> GetListLoaiAsync(string id)
        {
            var list = _context.CtLoaiTruyens.Where(p => p.IdBo == id.Trim()).ToList();
            List<string> listLoai = new List<string>();
            if (list.Count > 0)
            {
                foreach (var item in list)
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

        public async Task<int> CountByComicTypeId(string comicTypeId)
        {
            if (string.IsNullOrEmpty(comicTypeId))
                return 0;

            var count = await _context.BoTruyens.CountAsync(b => b.Listloai.Contains(comicTypeId));
            return count; // This will return 0 if no matches are found
        }

        public async Task<int> CountByAuthorId(string comicAuthId)
        {
            if (string.IsNullOrEmpty(comicAuthId))
                return 0;

            return await _context.BoTruyens.CountAsync(b => b.IdTg == comicAuthId);
        }


        public async Task<List<BoTruyen>> GetComicsByAuthorId(string authorId)
        {
            return await _context.BoTruyens
                .Where(bt => bt.IdTg == authorId)
                .ToListAsync();
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

        public async Task<IEnumerable<BoTruyen>> GetTrendingAsync()
        {
            return await _context.BoTruyens.OrderByDescending(p => p.TongLuotXem).ToListAsync();
        }

        public async Task<IEnumerable<BoTruyen>> GetRankingAsync()
        {
            return await _context.BoTruyens.OrderByDescending(p => p.TongLuotXem).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<BoTruyen>> GetAllAsyncByDay()
        {
            DateTime today = DateTime.Today;
            var booksWithDailyViews = await _context.BoTruyens
                .Select(b => new
                {
                    BoTruyen = b,
                    DailyViews = b.Chapters
                        .Where(c => c.ThoiGian.Date == today)
                        .Sum(c => c.TkLuotxem)
                })
                .OrderByDescending(x => x.DailyViews)
                .ToListAsync();
            return booksWithDailyViews.Select(x => x.BoTruyen).Take(10);
        }

        public async Task<IEnumerable<BoTruyen>> GetAllAsyncByFollow()
        {
            return await _context.BoTruyens.OrderByDescending(x => x.TkTheodoi).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<BoTruyen>> GetAllAsyncByRate()
        {
            var boTruyens = await _context.BoTruyens
            .AsNoTracking()
            .OrderByDescending(bt => bt.TkDanhgia)
            .ThenByDescending(bt => bt.TongLuotXem).Take(10)
            .ToListAsync();

            return boTruyens;
        }

        public async Task<IEnumerable<BoTruyen>> GetAllAsyncByMonth()
        {
            int today = DateTime.Now.Month;
            var booksWithDailyViews = await _context.BoTruyens
                .Select(b => new
                {
                    BoTruyen = b,
                    DailyViews = b.Chapters
                        .Where(c => c.ThoiGian.Date.Month == today)
                        .Sum(c => c.TkLuotxem)
                })
                .OrderByDescending(x => x.DailyViews)
                .ToListAsync();
            return booksWithDailyViews.Select(x => x.BoTruyen).Take(10);
        }

        public async Task<IEnumerable<BoTruyen>> SearchByNameAsync(string name)
        {
            return await _context.BoTruyens.Where(b => b.TenBo.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task<int> GetAllView()
        {
            var sum = await _context.BoTruyens.SumAsync(b => b.TongLuotXem);
            return sum;
        }

        public async Task<IEnumerable<Chapter>> GetListChapter(string id)
        {
            return await _context.Chapters.Where(c => c.IdBo == id).ToListAsync();
            
        }

        public async Task<int> CountChapterById(string id)
        {
            var chaps = await _context.Chapters.Where(c => c.IdBo == id).ToListAsync();
            return chaps.Count();
        }
    }
}
