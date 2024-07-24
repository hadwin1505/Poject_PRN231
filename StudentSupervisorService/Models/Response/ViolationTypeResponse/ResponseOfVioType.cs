using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.ViolationTypeResponse
{
    public class ResponseOfVioType
    {
        public int ViolationTypeId { get; set; }
        public string VioTypeName { get; set; } = null!;

        public int ViolationGroupId { get; set; }

        public string VioGroupName { get; set; } = null!;

        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
