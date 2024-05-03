using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class StaffActiveLog
{
    public string IdLog { get; set; } = null!;

    public string IdUser { get; set; } = null!;

    public byte IdPermissions { get; set; }

    public string? ChangeDescription { get; set; }

    public DateTime? TimeChanged { get; set; }

    public virtual StaffPermissionsDetail StaffPermissionsDetail { get; set; } = null!;
}
