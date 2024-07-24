using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class ViolationConfig
{
    public int ViolationConfigId { get; set; }

    public int ViolationTypeId { get; set; }

    public int? MinusPoints { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual ViolationType ViolationType { get; set; } = null!;
}
