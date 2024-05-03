using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class CtBoTruyen
{
    public string IdUser { get; set; } = null!;

    public string IbBo { get; set; } = null!;

    public bool Theodoi { get; set; }

    public byte DanhGia { get; set; }

    public string LsMoi { get; set; } = null!;

    public virtual BoTruyen IbBoNavigation { get; set; } = null!;

    public virtual KhachHang IdUserNavigation { get; set; } = null!;
}
