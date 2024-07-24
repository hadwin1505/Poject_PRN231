using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class StudentSupervisor
{
    public int StudentSupervisorId { get; set; }

    public int UserId { get; set; }

    public int? StudentInClassId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<PatrolSchedule> PatrolSchedules { get; set; } = new List<PatrolSchedule>();

    public virtual User User { get; set; } = null!;
    public virtual StudentInClass? StudentInClass { get; set; }
}
