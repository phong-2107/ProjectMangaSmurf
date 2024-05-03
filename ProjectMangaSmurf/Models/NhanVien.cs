using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class NhanVien
{
    public string IdUser { get; set; } = null!;

    public bool? StaffRole { get; set; }

    public virtual ICollection<StaffPermissionsDetail> StaffPermissionsDetails { get; set; } = new List<StaffPermissionsDetail>();

    public virtual User? User { get; set; }
}
