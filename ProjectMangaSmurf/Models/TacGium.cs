using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class TacGium
{
    public string IdTg { get; set; } = null!;

    public string? TenTg { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<BoTruyen> BoTruyens { get; set; } = new List<BoTruyen>();
}
