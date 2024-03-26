using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class CtLoaiTruyen
{
    public string IdLoai { get; set; } = null!;

    public string IdBo { get; set; } = null!;

    public bool Active { get; set; }

    public virtual BoTruyen IdBoNavigation { get; set; } = null!;

    public virtual LoaiTruyen IdLoaiNavigation { get; set; } = null!;
}
