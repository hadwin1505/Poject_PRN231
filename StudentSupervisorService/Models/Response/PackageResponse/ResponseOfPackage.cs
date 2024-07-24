using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.PackageResponse
{
    public class ResponseOfPackage
    {
        public int PackageId { get; set; }
        public int PackageTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? TotalStudents { get; set; }

        public int? TotalViolations { get; set; }

        public int? Price { get; set; }

        public string? Status { get; set; }
    }
}
