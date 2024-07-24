using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.StudentResponse
{
    public class StudentResponse
    {
        public int StudentId { get; set; }
        public int SchoolId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
