using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Models.Response.CheckoutResponse
{
    public class CheckoutResponse
    {
        public string? Code { get; set; }
        public string? Id { get; set; }
        public bool? Cancel { get; set; }
        public string? Status { get; set; }
        public long OrderCode { get; set; }
    }
}
