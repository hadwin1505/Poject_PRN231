using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.RegisteredSchoolRequest
{
    public class RegisteredSchoolCreateRequest
    {
        // Fields của HighSchool
        public string? SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? WebURL { get; set; }
        // Fields của RegisteredSchool
        public DateTime RegisteredDate { get; set; }
        public string? Description { get; set; }
    }

    public class RegisteredSchoolUpdateRequest
    {
        // Fields của RegisteredSchool
        public int RegisteredId { get; set; }
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
