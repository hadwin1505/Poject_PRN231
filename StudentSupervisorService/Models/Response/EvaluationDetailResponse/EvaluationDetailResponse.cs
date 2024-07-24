using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.EvaluationDetailResponse
{
    public class EvaluationDetailResponse
    {
        public int EvaluationDetailId { get; set; }
        public int ClassId { get; set; }
        public int EvaluationId { get; set; }
        public string? Status { get; set; }
    }
}
