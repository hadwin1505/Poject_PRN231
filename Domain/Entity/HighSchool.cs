using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class HighSchool
{
    public int SchoolId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? WebUrl { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ClassGroup> ClassGroups { get; set; } = new List<ClassGroup>();

    public virtual ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();

    public virtual ICollection<RegisteredSchool> RegisteredSchools { get; set; } = new List<RegisteredSchool>();

    public virtual ICollection<SchoolYear> SchoolYears { get; set; } = new List<SchoolYear>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<ViolationGroup> ViolationGroups { get; set; } = new List<ViolationGroup>();
}
