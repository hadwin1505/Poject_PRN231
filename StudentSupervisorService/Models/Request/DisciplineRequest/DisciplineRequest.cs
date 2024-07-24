using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.DisciplineRequest
{
    public class DisciplineCreateRequest
    {
        public int ViolationId { get; set; }
        public int? PennaltyId { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class DisciplineUpdateRequest
    {
        public int DisciplineId { get; set; }
        public int? ViolationId { get; set; }
        public int? PennaltyId { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DisciplineStatusEnums? Status { get; set; }
    }
}
