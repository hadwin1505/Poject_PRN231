using Domain.Entity;
using Domain.Enums.Status;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.ViolationRequest
{
    public class RequestOfCreateViolation
    {
        [Required(ErrorMessage = "The ClassId field is required.")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "The ViolationTypeId field is required.")]
        public int ViolationTypeId { get; set; }

        [Required(ErrorMessage = "The StudentInClassId field is required.")]
        public int StudentInClassId { get; set; }

        public int? TeacherId { get; set; }

        [Required(ErrorMessage = "The ViolationName field is required.")]
        public string ViolationName { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "The Date field is required.")]
        public DateTime Date { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class RequestOfUpdateViolation
    {
        //public int ViolationId { get; set; }
        public int ClassId { get; set; }

        public int ViolationTypeId { get; set; }

        public int? StudentInClassId { get; set; }

        public int? TeacherId { get; set; }

        public string ViolationName { get; set; } = null!;

        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
