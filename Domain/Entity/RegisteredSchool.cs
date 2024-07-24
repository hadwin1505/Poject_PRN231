using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class RegisteredSchool
{
    public int RegisteredId { get; set; }

    public int SchoolId { get; set; }

    public DateTime RegisteredDate { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public virtual HighSchool School { get; set; } = null!;
}
