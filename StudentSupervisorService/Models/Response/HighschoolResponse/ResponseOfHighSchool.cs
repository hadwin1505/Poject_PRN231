using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.HighschoolResponse
{
    public class ResponseOfHighSchool
    {
        public int SchoolId { get; set; }
        public string? Code { get; set; }

        public string Name { get; set; } = null!;

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? WebUrl { get; set; }

        public string Status { get; set; } = null!;
    }
}
