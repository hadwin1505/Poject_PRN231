using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.DisciplineResponse
{
    public class DisciplineResponse
    {
        public int DisciplineId { get; set; }
        public int ViolationId { get; set; }
        public int PennaltyId { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}
