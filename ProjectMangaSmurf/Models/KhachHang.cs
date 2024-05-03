using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class KhachHang
{
    public string IdUser { get; set; } = null!;

    public string? GoogleAccount { get; set; }

    public string? FacebookAccount { get; set; }

    public string? IdAvatar { get; set; }

    public int? TicketSalary { get; set; }

    public bool? ActivePremium { get; set; }

    public byte? ActiveStats { get; set; }

    public virtual ICollection<CtBoTruyen> CtBoTruyens { get; set; } = new List<CtBoTruyen>();

    public virtual ICollection<CtHoatDong> CtHoatDongs { get; set; } = new List<CtHoatDong>();

    public virtual ICollection<CustomerLogin> CustomerLogins { get; set; } = new List<CustomerLogin>();

    public virtual Avatar? IdAvatarNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual Payment? Payment { get; set; }
}
