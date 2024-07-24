using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.PatrolScheduleResponse
{
    public class PatrolScheduleResponse
    {
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public int SupervisorId { get; set; }
        public string? SupervisorName { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? Status { get; set; }
    }
}
