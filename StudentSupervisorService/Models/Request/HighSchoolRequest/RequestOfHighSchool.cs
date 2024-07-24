using Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.HighSchoolRequest
{
    public class RequestOfHighSchool
    {
        [Required(ErrorMessage = "The Code field is required.")]
        public string? Code { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "The City field is required.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "The Address field is required.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "The Phone field is required.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "The WebUrl field is required.")]
        public string? WebUrl { get; set; }
    }
}
