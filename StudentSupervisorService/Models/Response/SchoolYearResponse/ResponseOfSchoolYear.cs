using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.SchoolYearResponse
{
    public class ResponseOfSchoolYear
    {
        public int SchoolYearId { get; set; }

        public short Year { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
