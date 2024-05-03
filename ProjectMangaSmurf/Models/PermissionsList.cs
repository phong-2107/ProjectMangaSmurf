using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class PermissionsList
{
    public byte IdPermissions { get; set; }

    public byte? ParentPermissions { get; set; }

    public string? PermissionsName { get; set; }

    public string? Description { get; set; }

    public byte? PermissionsStats { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<StaffPermissionsDetail> StaffPermissionsDetails { get; set; } = new List<StaffPermissionsDetail>();
}
