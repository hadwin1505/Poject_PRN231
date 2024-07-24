using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public byte RoleId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
