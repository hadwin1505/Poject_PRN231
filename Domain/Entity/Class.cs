using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Class
{
    public int ClassId { get; set; }

    public int SchoolYearId { get; set; }

    public int ClassGroupId { get; set; }
    public int? TeacherId { get; set; }

    public string? Code { get; set; }
    public int Grade { get; set; }

    public string Name { get; set; } = null!;

    public int TotalPoint { get; set; }

    public virtual ClassGroup ClassGroup { get; set; } = null!;

    public virtual ICollection<EvaluationDetail> EvaluationDetails { get; set; } = new List<EvaluationDetail>();

    public virtual ICollection<PatrolSchedule> PatrolSchedules { get; set; } = new List<PatrolSchedule>();

    public virtual SchoolYear SchoolYear { get; set; } = null!;

    public virtual Teacher? Teacher { get; set; } = null!;

    public virtual ICollection<StudentInClass> StudentInClasses { get; set; } = new List<StudentInClass>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
