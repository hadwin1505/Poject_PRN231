﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.StudentInClassRequest
{
    public class StudentInClassCreateRequest
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public DateTime? EnrollDate { get; set; }
        public bool IsSupervisor { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumberOfViolation { get; set; }
    }

    public class StudentInClassUpdateRequest
    {
        public int StudentInClassId { get; set; }
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? EnrollDate { get; set; }
        public bool? IsSupervisor { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumberOfViolation { get; set; }
    }
}
