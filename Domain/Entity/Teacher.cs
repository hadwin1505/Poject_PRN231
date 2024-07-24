using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int UserId { get; set; }

    public int SchoolId { get; set; }

    public bool Sex { get; set; }

    public virtual ICollection<PatrolSchedule> PatrolSchedules { get; set; } = new List<PatrolSchedule>();

    public virtual HighSchool School { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
