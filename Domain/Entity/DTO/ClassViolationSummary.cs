using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.DTO
{
    public class ClassViolationSummary
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public int ViolationCount { get; set; }
    }
}
