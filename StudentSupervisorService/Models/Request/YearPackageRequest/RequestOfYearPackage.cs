using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.YearPackageRequest
{
    public class RequestOfYearPackage
    {
        [Required(ErrorMessage = "The SchoolYearId field is required.")]
        public int SchoolYearId { get; set; }
        [Required(ErrorMessage = "The PackageId field is required.")]
        public int PackageId { get; set; }
        [Required(ErrorMessage = "The NumberOfStudent field is required.")]
        public int? NumberOfStudent { get; set; }
    }
}
