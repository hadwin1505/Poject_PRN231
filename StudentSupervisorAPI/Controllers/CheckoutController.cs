using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Net.payOS;
using Net.payOS.Types;
using StudentSupervisorService.Authentication;
using StudentSupervisorService.Models.Request.CheckoutRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.CheckoutResponse;
using StudentSupervisorService.Models.Response.OrderResponse;
using StudentSupervisorService.PayOSConfig;
using StudentSupervisorService.Service;
using System;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private CheckoutService _checkoutService;
        private IAuthentication _authenService;
        private PayOS _payOS;

        public CheckoutController(CheckoutService checkoutService, IAuthentication authenService, PayOS payOS)
        {
            _checkoutService = checkoutService;
            _authenService = authenService;
            _payOS = payOS;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SCHOOL_ADMIN")]
        [HttpPost]
        public async Task<ActionResult<DataResponse<CreatePaymentResult>>> CreatePaymentLink([FromBody] CreateCheckoutRequest request)
        {
            try
            {
                // Lấy userId từ JWT
                var userId = _authenService.GetUserIdFromContext(HttpContext);
                if (userId == null)
                {
                    return Unauthorized("Không lấy được UserID từ JWT");
                }
                var checkoutResponse = await _checkoutService.CreateCheckout(userId, request);
                return Ok(checkoutResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("verify")]
        public async Task<ActionResult<DataResponse<OrderResponse>>> VerifyTransaction([FromQuery] CheckoutResponse queryParams)
        {
            try
            {
                var orderResponse = await _checkoutService.VerifyTransaction(queryParams);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{orderCode}")]
        public async Task<ActionResult> check(int orderCode)
        {
            try
            {
                PaymentLinkInformation infor = await _payOS.getPaymentLinkInformation(orderCode);
                return Ok(infor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
