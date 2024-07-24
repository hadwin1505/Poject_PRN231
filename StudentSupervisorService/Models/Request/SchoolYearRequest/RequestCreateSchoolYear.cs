using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.SchoolYearRequest
{
    public class RequestCreateSchoolYear
    {
        [Required(ErrorMessage = "The SchoolId field is required.")]
        public int SchoolId { get; set; }
        [Required(ErrorMessage = "The Year field is required.")]
        public short Year { get; set; }
        [Required(ErrorMessage = "The StartDate field is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "The EndDate field is required.")]
        public DateTime EndDate { get; set; }
    }
}
