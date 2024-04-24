using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public class HopDong
{
    public string IdHd { get; set; } = null!;

    public DateTime Ngaylap { get; set; }

    public string IdKh { get; set; } = null!;
    public string NoiDung { get; set; }
    public int MaGiaoDich { get; set; }
    public decimal GtThanhtoan { get; set; }

    public virtual KhachHang IdKhNavigation { get; set; } = null!;
}
