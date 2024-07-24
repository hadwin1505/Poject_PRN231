using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.PackageRequest
{
    public class PackageRequest
    {
        [Required(ErrorMessage = "The PackageTypeId field is required.")]
        public int PackageTypeId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } 

        [Required(ErrorMessage = "The Description field is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The TotalStudents field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "TotalStudents cannot be negative.")]
        public int? TotalStudents { get; set; }

        [Required(ErrorMessage = "The TotalViolations field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "TotalViolations cannot be negative.")]
        public int? TotalViolations { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public int? Price { get; set; }

    }
}
