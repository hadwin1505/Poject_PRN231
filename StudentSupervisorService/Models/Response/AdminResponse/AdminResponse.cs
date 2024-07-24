using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.AdminResponse
{
    public class AdminResponse
    {
        public int AdminId { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Phone { get; set; }

        public byte RoleId { get; set; }

        public string Status { get; set; } = null!;
    }
}
