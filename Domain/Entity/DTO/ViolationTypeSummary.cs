using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.DTO
{
    public class ViolationTypeSummary
    {
        public int ViolationTypeId { get; set; }
        public string ViolationTypeName { get; set; } = null!;
        public int ViolationCount { get; set; }
    }
}
