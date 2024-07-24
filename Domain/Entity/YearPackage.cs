using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class YearPackage
{
    public int YearPackageId { get; set; }

    public int SchoolYearId { get; set; }

    public int PackageId { get; set; }

    public int? NumberOfStudent { get; set; }

    public string? Status { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual SchoolYear SchoolYear { get; set; } = null!;
}
