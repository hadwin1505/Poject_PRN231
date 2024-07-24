using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Request.OrderRequest
{
    public class OrderCreateRequest
    {
        [Required(ErrorMessage = "The UserId field is required.")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "The PackageId field is required.")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "The OrderCode field is required.")]
        public int OrderCode { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Total field is required.")]
        public int Total { get; set; }
        public int? AmountPaid { get; set; }
        public int? AmountRemaining { get; set; }
        public string? CounterAccountBankName { get; set; }
        public string? CounterAccountNumber { get; set; }
        public string? CounterAccountName { get; set; }
    }

    public class OrderUpdateRequest
    {
        [Required(ErrorMessage = "The OrderCode field is required.")]
        public int OrderCode { get; set; }
        public string? Description { get; set; }
        public int? AmountPaid { get; set; }
        public int? AmountRemaining { get; set; }
        public string? CounterAccountBankName { get; set; }
        public string? CounterAccountNumber { get; set; }
        public string? CounterAccountName { get; set; }
        public string? Status { get; set; }
    }
}
