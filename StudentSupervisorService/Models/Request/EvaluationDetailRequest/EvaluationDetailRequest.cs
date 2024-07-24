using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.EvaluationDetailRequest
{
    public class EvaluationDetailCreateRequest
    {
        public int ClassId { get; set; }
        public int EvaluationId { get; set; }
    }

    public class EvaluationDetailUpdateRequest
    {
        public int EvaluationDetailId { get; set; }
        public int? ClassId { get; set; }
        public int? EvaluationId { get; set; }
        public EvaluationDetailStatusEnums? Status { get; set; }
    }
}
