using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Penalty
{
    public int PenaltyId { get; set; }

    public int SchoolId { get; set; }
    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual HighSchool School { get; set; } = null!;
}
