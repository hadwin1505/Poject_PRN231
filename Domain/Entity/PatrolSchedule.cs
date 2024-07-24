using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class PatrolSchedule
{
    public int ScheduleId { get; set; }

    public int ClassId { get; set; }

    public int SupervisorId { get; set; }

    public int TeacherId { get; set; }

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public string? Status { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual StudentSupervisor Supervisor { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
