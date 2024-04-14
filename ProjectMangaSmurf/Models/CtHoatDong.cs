using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public class CtHoatDong
{
    public string IdBo { get; set; } = null!;

    public int SttChap { get; set; }

    public string IdKh { get; set; } = null!;

    public bool? TtDoc { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;

    public virtual KhachHang IdKhNavigation { get; set; } = null!;
}
