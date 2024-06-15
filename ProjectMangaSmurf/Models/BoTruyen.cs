using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class BoTruyen
{
    public string IdBo { get; set; } = null!;

    public string TenBo { get; set; } = null!;

    public byte Dotuoi { get; set; }

    public string IdTg { get; set; } = null!;

    public string Mota { get; set; } = null!;

    public double TkDanhgia { get; set; }

    public int TkTheodoi { get; set; }

    public int TongLuotXem { get; set; }

    public string? AnhBia { get; set; } 

    public string? AnhBanner { get; set; } 

    public bool TtPemium { get; set; }

    public byte TrangThai { get; set; }

    public bool Active { get; set; }

    public string? Listloai { get; set; }

    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    public virtual ICollection<CtBoTruyen> CtBoTruyens { get; set; } = new List<CtBoTruyen>();

    public virtual ICollection<CtLoaiTruyen> CtLoaiTruyens { get; set; } = new List<CtLoaiTruyen>();

    public virtual TacGium? IdTgNavigation { get; set; } 
}
