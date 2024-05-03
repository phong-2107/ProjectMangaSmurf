using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class ServicePackConfig
{
    public string IdPack { get; set; } = null!;

    public string? ParentPack { get; set; }

    public string? PackName { get; set; }

    public byte? TicketSalary { get; set; }

    public DateTimeOffset? ActivateTimeService { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Discount { get; set; }

    public string? Description { get; set; }

    public byte? PackActiveStats { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
