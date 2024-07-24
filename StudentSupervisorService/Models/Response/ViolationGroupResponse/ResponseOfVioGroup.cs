using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.ViolationGroupResponse
{
    public class ResponseOfVioGroup
    {
        public int ViolationGroupId { get; set; }

        public int? SchoolId { get; set; }
        public string SchoolName { get; set; } = null!;
        public string? VioGroupCode { get; set; }

        public string VioGroupName { get; set; } = null!;

        public string? Description { get; set; }

        public string? Status { get; set; }
    }
}
