using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Violation
{
    public int ViolationId { get; set; }

    public int ClassId { get; set; }

    public int ViolationTypeId { get; set; }

    public int? StudentInClassId { get; set; }

    public int? TeacherId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string Status { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Discipline> Disciplines { get; set; } = new List<Discipline>();

    public virtual ICollection<ImageUrl> ImageUrls { get; set; } = new List<ImageUrl>();

    public virtual StudentInClass? StudentInClass { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual ViolationType ViolationType { get; set; } = null!;
}
