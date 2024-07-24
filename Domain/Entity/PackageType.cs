using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PackageType
    {
        public int PackageTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}
