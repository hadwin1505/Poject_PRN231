using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.TeacherResponse
{
    public class TeacherResponse
    {
        public int TeacherId { get; set; }
        public string Code { get; set; }
        public string TeacherName { get; set; }
        public string SchoolName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool Sex { get; set; }
        public string? Address { get; set; }
        public byte RoleId { get; set; }
        public string Status { get; set; } = null!;

    }
}
