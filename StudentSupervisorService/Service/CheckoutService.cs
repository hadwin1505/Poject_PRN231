using Net.payOS.Types;
using StudentSupervisorService.Models.Request.CheckoutRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.CheckoutResponse;
using StudentSupervisorService.Models.Response.OrderResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service
{
    public interface CheckoutService
    {
        Task<DataResponse<CreatePaymentResult>> CreateCheckout(int? userIdFromJWT, CreateCheckoutRequest request);
        Task<DataResponse<OrderResponse>> VerifyTransaction(CheckoutResponse request);
    }
}
