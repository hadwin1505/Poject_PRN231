using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Discipline
{
    public int DisciplineId { get; set; }

    public int ViolationId { get; set; }

    public int? PennaltyId { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Penalty? Pennalty { get; set; } = null!;

    public virtual Violation Violation { get; set; } = null!;
}
