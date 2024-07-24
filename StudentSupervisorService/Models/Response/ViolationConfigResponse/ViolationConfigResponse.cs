using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.ViolationConfigResponse
{
    public class ViolationConfigResponse
    {
        public int ViolationConfigId { get; set; }

        public int ViolationTypeId { get; set; }

        public string ViolationTypeName { get; set; } = null!;

        public int? MinusPoints { get; set; }

        public string? Description { get; set; }
        public string Status { get; set; } = null!;
    }
}
