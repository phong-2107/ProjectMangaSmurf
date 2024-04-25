using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMangaSmurf.Models;

public class Premium
{


    [Range(1, 999999, ErrorMessage = "The price must be between $1 and $999999.")]
    public decimal GiaThanh { get; set; }

    public bool Active { get; set; }
    // Other properties
}