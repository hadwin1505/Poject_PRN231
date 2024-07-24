using Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.StudentSupervisorRequest
{
    public class StudentSupervisorRequest
    {
        [Required(ErrorMessage = "The StudentInClassId field is required.")]
        public int? StudentInClassId { get; set; }

        //[Required(ErrorMessage = "The IsSupervisor field is required.")]
        //public bool? IsSupervisor { get; set; }

        [Required(ErrorMessage = "The SchoolId field is required.")]
        public int SchoolId { get; set; }

        [Required(ErrorMessage = "The Code field is required.")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "The SupervisorName field is required.")]
        public string SupervisorName { get; set; } = null!;

        [RegularExpression(@"^84[0-9]{8,9}$|^[0-9]{8,9}$", ErrorMessage = "The phone number must be 8 or 9 digits, with optional '84' country code prefix.")]
        [Required(ErrorMessage = "The Phone field is required.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "The Address field is required.")]
        public string? Address { get; set; } = null!;

        [Required(ErrorMessage = "The Description field is required.")]
        public string? Description { get; set; }

    }
}
