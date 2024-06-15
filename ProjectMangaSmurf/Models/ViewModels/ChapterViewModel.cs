namespace ProjectMangaSmurf.Models.ViewModels
{
    public class ChapterViewModel
    {
        public string IdBo { get; set; } = null!;
        public int SttChap { get; set; }
        public string TenChap { get; set; } = null!;
        public string? ChapterContent { get; set; }
        public DateTime ThoiGian { get; set; } = DateTime.Now;
        public byte? TrangThai { get; set; }
        public bool TtPemium { get; set; }
        public byte? TicketCost { get; set; }
        public int TkLuotxem { get; set; }
        public bool Active { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
