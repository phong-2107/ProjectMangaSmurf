using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class User
{
    public string IdUser { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? Birth { get; set; }

    public byte? Gender { get; set; }

    public DateTimeOffset? TimeCreated { get; set; }

    public DateTimeOffset? TimeUpdated { get; set; }

    public bool? Active { get; set; }

    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual KhachHang KhachHang { get; set; } = null!;
}
