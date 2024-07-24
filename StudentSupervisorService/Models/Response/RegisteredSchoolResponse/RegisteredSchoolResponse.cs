using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.RegisteredSchoolResponse
{
    public class RegisteredSchoolResponse
    {
        // Fields của RegisteredSchool
        public int RegisteredId { get; set; }
        public int SchoolId { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        // Fields của HighSchool
        public string? SchoolCode { get; set; }
        public string? SchoolName { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? WebURL { get; set; }
    }
}
