using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Evaluation
{
    public int EvaluationId { get; set; }

    public int SchoolYearId { get; set; }

    public int? ViolationConfigId { get; set; }

    public string? Description { get; set; }

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public short Point { get; set; }

    public virtual ICollection<EvaluationDetail> EvaluationDetails { get; set; } = new List<EvaluationDetail>();

    public virtual SchoolYear SchoolYear { get; set; } = null!;

    public virtual ViolationConfig? ViolationConfig { get; set; }
}
