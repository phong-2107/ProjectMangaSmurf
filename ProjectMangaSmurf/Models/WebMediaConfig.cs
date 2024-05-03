using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class WebMediaConfig
{
    public int IdConfig { get; set; }

    public string? ConfigValue { get; set; }

    public string? ConfigTitle { get; set; }

    public bool? Active { get; set; }
}
