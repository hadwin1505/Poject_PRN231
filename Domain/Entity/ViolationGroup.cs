using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class ViolationGroup
{
    public int ViolationGroupId { get; set; }

    public int? SchoolId { get; set; }
    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual HighSchool? School { get; set; }

    public virtual ICollection<ViolationType> ViolationTypes { get; set; } = new List<ViolationType>();
}
