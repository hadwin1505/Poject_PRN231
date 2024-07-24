using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.ClassRequest
{
    public class ClassCreateRequest
    {
        public int SchoolYearId { get; set; }
        public int ClassGroupId { get; set; }
        public int? TeacherId { get; set; }
        public string? Code { get; set; }
        public int Grade { get; set; }
        public string Name { get; set; }
        public int TotalPoint { get; set; }
    }

    public class ClassUpdateRequest
    {
        public int ClassId { get; set; }
        public int? SchoolYearId { get; set; }
        public int? ClassGroupId { get; set; }
        public int? TeacherId { get; set; }
        public string? Code { get; set; }
        public int? Grade { get; set; }
        public string? Name { get; set; }
        public int? TotalPoint { get; set; }
    }
}
