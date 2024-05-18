using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class Payment
{
    public string IdPayment { get; set; } = null!;

    public string? IdUser { get; set; }

    public string? IdPack { get; set; }

    public decimal? PayAmount { get; set; }

    public byte? PayMethod { get; set; }

    public byte? PayStats { get; set; }

    public DateTimeOffset? PayDate { get; set; }

    public DateTimeOffset? ExpiresTime { get; set; }

    public virtual ServicePackConfig? IdPackNavigation { get; set; }

    public virtual KhachHang? IdUserNavigation { get; set; }
}
