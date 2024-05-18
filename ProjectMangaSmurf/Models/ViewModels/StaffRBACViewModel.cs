using ProjectMangaSmurf.Models;

public class StaffRBACViewModel
{
    public string IdUser { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public byte? Gender { get; set; }
    public bool? StaffRole { get; set; }
    public bool? Active {  get; set; }

    public List<byte> SelectedPermissions { get; set; } // IDs of selected permissions
    public IEnumerable<PermissionsList> AvailablePermissions { get; set; } // List of all permissions
}

