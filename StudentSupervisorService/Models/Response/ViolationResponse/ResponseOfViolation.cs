using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.ViolationResponse
{
    public class ResponseOfViolation
    {
        public int ViolationId { get; set; }
        public int ClassId { get; set; }
        public int? StudentInClassId { get; set; }
        public string? StudentName { get; set; }
        public int ViolationTypeId { get; set; }
        public string? ViolationTypeName { get; set; }
        public int ViolationGroupId { get; set; }
        public string? ViolationGroupName { get; set; }
        public int? TeacherId { get; set; }
        public string ViolationName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public string? Status { get; set; }
    }
}
