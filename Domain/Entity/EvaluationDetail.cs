using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class EvaluationDetail
{
    public int EvaluationDetailId { get; set; }

    public int ClassId { get; set; }

    public int EvaluationId { get; set; }

    public string? Status { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Evaluation Evaluation { get; set; } = null!;
}
