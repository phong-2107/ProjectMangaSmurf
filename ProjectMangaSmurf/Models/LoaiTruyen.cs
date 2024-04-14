using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public class LoaiTruyen
{
    public string IdLoai { get; set; } = null!;

    public string TenLoai { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<CtLoaiTruyen> CtLoaiTruyens { get; set; } = new List<CtLoaiTruyen>();
}
