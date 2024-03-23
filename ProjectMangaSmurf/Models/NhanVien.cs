using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class NhanVien
{
    public string IdNv { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string Taikhoan { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public bool LoaiNv { get; set; }

    public bool Active { get; set; }
}
