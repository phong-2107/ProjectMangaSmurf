using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class Chapter
{
    public string IdBo { get; set; } = null!;

    public int SttChap { get; set; }

    public string TenChap { get; set; } = null!;

    public string? ChapterContent { get; set; }

    public DateTime ThoiGian { get; set; }

    public byte? TrangThai { get; set; }

    public bool TtPemium { get; set; }

    public byte? TicketCost { get; set; }

    public int TkLuotxem { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<CtChapter> CtChapters { get; set; } = new List<CtChapter>();

    public virtual ICollection<CtHoatDong> CtHoatDongs { get; set; } = new List<CtHoatDong>();

    public virtual BoTruyen IdBoNavigation { get; set; } = null!;
}
