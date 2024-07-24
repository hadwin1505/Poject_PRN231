using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int PackageId { get; set; }

        public int OrderCode { get; set; }

        public string? Description { get; set; }

        public int Total { get; set; }

        public int? AmountPaid { get; set; }

        public int? AmountRemaining { get; set; }

        public string? CounterAccountBankName { get; set; }

        public string? CounterAccountNumber { get; set; }

        public string? CounterAccountName { get; set; }

        public DateTime? Date { get; set; }

        public string Status { get; set; } = null!;

        public virtual Package Package { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
