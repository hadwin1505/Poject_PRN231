using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class ViolationType
{
    public int ViolationTypeId { get; set; }

    public int ViolationGroupId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ViolationConfig> ViolationConfigs { get; set; } = new List<ViolationConfig>();

    public virtual ViolationGroup ViolationGroup { get; set; } = null!;

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
