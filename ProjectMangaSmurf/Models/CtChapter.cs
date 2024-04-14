using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public class CtChapter
{
    public int SoTrang { get; set; }

    public string IdBo { get; set; } = null!;

    public int SttChap { get; set; }

    public string AnhTrang { get; set; } = null!;

    public bool Active { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;
}
