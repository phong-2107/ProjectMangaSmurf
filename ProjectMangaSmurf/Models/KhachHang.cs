using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class KhachHang 
{
    public string IdKh { get; set; } = null!;

    public string? TenKh { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? LienketFb { get; set; }

    public string? LienketGg { get; set; }

    public string Taikhoan { get; set; }

    public string Matkhau { get; set; } = null!;

    public bool TtPremium { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<CtBoTruyen> CtBoTruyens { get; set; } = new List<CtBoTruyen>();

    public virtual ICollection<CtHoatDong> CtHoatDongs { get; set; } = new List<CtHoatDong>();

    public virtual ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
}
