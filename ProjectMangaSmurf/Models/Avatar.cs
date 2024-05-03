using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class Avatar
{
    public string IdAvatar { get; set; } = null!;

    public string? AvatarContent { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
