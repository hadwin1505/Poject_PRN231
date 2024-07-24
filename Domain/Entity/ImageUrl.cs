using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class ImageUrl
{
    public int ImageUrlid { get; set; }

    public int ViolationId { get; set; }

    public string? Url { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual Violation Violation { get; set; } = null!;
}
