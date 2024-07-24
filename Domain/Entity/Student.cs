using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Student
{
    public int StudentId { get; set; }

    public int SchoolId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? Sex { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual HighSchool School { get; set; } = null!;

    public virtual ICollection<StudentInClass> StudentInClasses { get; set; } = new List<StudentInClass>();
}
