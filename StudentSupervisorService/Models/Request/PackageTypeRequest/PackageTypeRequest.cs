using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.PackageTypeRequest
{
    public class PackageTypeRequest
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "The Description field is required.")]
        public string? Description { get; set; }
    }
}
