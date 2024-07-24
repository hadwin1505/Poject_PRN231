using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.ViolationGroupRequest
{
    public class RequestOfVioGroup
    {
        [Required(ErrorMessage = "The SchoolId field is required.")]
        public int? SchoolId { get; set; }

        [Required(ErrorMessage = "The VioGroupCode field is required.")]
        public string? VioGroupCode { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string VioGroupName { get; set; } = null!;

        [Required(ErrorMessage = "The Description field is required.")]
        public string? Description { get; set; }
    }
}
