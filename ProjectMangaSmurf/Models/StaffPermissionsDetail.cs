using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class StaffPermissionsDetail
{
    public string IdUser { get; set; } = null!;

    public byte IdPermissions { get; set; }

    public bool? Active { get; set; }

    public virtual PermissionsList IdPermissionsNavigation { get; set; } = null!;

    public virtual NhanVien IdUserNavigation { get; set; } = null!;

    public virtual ICollection<StaffActiveLog> StaffActiveLogs { get; set; } = new List<StaffActiveLog>();
}
