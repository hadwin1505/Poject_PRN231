using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class ClassGroup
{
    public int ClassGroupId { get; set; }

    public int? SchoolId { get; set; }

    public string Hall { get; set; } = null!;

    public int? Slot { get; set; }

    public TimeSpan? Time { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual HighSchool? School { get; set; }
}
