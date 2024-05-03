using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class CustomerLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? IdUser { get; set; }

    public virtual KhachHang? IdUserNavigation { get; set; }
}
