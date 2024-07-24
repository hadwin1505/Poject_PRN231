using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.PatrolScheduleRequest
{
    public class PatrolScheduleCreateRequest
    {
        public int ClassId { get; set; }
        public int SupervisorId { get; set; }
        public int TeacherId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class PatrolScheduleUpdateRequest
    {
        public int ScheduleId { get; set; }
        public int? ClassId { get; set; }
        public int? SupervisorId { get; set; }
        public int? TeacherId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public PatrolScheduleStatusEnums? Status { get; set; }
    }
}
