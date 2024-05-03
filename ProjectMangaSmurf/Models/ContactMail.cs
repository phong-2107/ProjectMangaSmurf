using System;
using System.Collections.Generic;

namespace ProjectMangaSmurf.Models;

public partial class ContactMail
{
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;
}
