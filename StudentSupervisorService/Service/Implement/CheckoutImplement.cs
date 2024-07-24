using AutoMapper;
using Infrastructures.Interfaces.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using StudentSupervisorService.Models.Response.AdminResponse;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.CheckoutResponse;
using StudentSupervisorService.PayOSConfig;
using Net.payOS.Types;
using StudentSupervisorService.Models.Request.CheckoutRequest;
using StudentSupervisorService.Models.Response.OrderResponse;
using StudentSupervisorService.Models.Request.OrderRequest;
using Domain.Enums.Status;


namespace StudentSupervisorService.Service.Implement
{
    public class CheckoutImplement : CheckoutService
    {
        private readonly OrderService _orderService;
        private readonly PayOS _payOS;
        private readonly PayOSConfig.PayOSConfig _payOSConfig;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CheckoutImplement(IUnitOfWork unitOfWork, IMapper mapper, OrderService orderService,
            PayOS payOS, PayOSConfig.PayOSConfig payOSConfig)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOS = payOS;
            _payOSConfig = payOSConfig;
            _orderService = orderService;
        }

        public async Task<DataResponse<CreatePaymentResult>> CreateCheckout(int? userIdFromJWT, CreateCheckoutRequest request)
        {
            var response = new DataResponse<CreatePaymentResult>();
            try
            {
                // kiểm tra xem package có tồn tại không
                var existingPackage = await _unitOfWork.Package.GetPackageById(request.PackageID);
                if (existingPackage is null)
                {
                    response.Message = "Gói Package không tồn tại";
                    response.Success = false;
                    return response;
                }
                // tạo thông tin thanh toán
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                List<ItemData> items = new List<ItemData> { new ItemData(existingPackage.Name, 1, existingPackage.Price) };
                PaymentData paymentData = new PaymentData(
                    orderCode,
                    existingPackage.Price,
                    "Thanh toan don hang",
                    items,
                    _payOSConfig.GetCancelUrl(),
                    _payOSConfig.GetReturnUrl());
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                // tạo object OrderCreateRequest để insert Order xuống DB
                OrderCreateRequest orderCreateRequest = new OrderCreateRequest
                {
                    UserId = userIdFromJWT,
                    PackageId = request.PackageID,
                    OrderCode = orderCode,
                    Description = "Thanh toán cho " + existingPackage.Name,
                    Total = existingPackage.Price,
                    AmountPaid = null,
                    AmountRemaining = null,
                    CounterAccountBankName = null,
                    CounterAccountNumber = null,
                    CounterAccountName = null
                };
                await _orderService.CreateOrder(orderCreateRequest);

                response.Data = createPayment;
                response.Message = "success";
                response.Success = true;
            } catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Oops! Some thing went wrong.\n" + ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<OrderResponse>> VerifyTransaction(CheckoutResponse queryParams)
        {
            var response = new DataResponse<OrderResponse>();
            try
            {
                if (queryParams.OrderCode == null)
                {
                    response.Data = "Empty";
                    response.Message = "OrderCode không tồn tại";
                    response.Success = false;
                    return response;
                }

                PaymentLinkInformation paymentLinkInfomation = await _payOS.getPaymentLinkInformation(queryParams.OrderCode);
                if (paymentLinkInfomation is null)
                {
                    response.Data = "Empty";
                    response.Message = "PayOS không tìm thấy thông tin thanh toán";
                    response.Success = false;
                    return response;
                }

                // người dùng thanh toán thành công
                if (queryParams.Status == "PAID")
                {
                    // cập nhật status Order thành PAID
                    OrderUpdateRequest orderUpdateRequest = new OrderUpdateRequest
                    {
                        OrderCode = (int)queryParams.OrderCode,
                        AmountPaid = paymentLinkInfomation.amountPaid,
                        AmountRemaining = 0,
                        CounterAccountBankName = paymentLinkInfomation.transactions[0].counterAccountBankName,
                        CounterAccountNumber = paymentLinkInfomation.transactions[0].counterAccountNumber,
                        CounterAccountName = paymentLinkInfomation.transactions[0].counterAccountName,
                        Status = OrderStatusEnum.PAID.ToString()
                    };

                    // thêm 1 Package vào YearlyPackage của HighSchool

                    var updated = await _orderService.UpdateOrder(orderUpdateRequest);
                    response.Data = updated;
                    response.Message = "Xác nhận hóa đơn (Order) thành công!";
                    response.Success = true;
                }
                // người dùng hủy thanh toán
                else if (queryParams.Status == "CANCELLED")
                {
                    // cập nhật status Order thành CANCELLED
                    OrderUpdateRequest orderUpdateRequest = new OrderUpdateRequest
                    {
                        OrderCode = (int)queryParams.OrderCode,
                        AmountPaid = 0,
                        AmountRemaining = paymentLinkInfomation.amountRemaining,
                        CounterAccountBankName = null,
                        CounterAccountNumber = null,
                        CounterAccountName = null,
                        Status = OrderStatusEnum.CANCELLED.ToString()
                    };

                    var updated = await _orderService.UpdateOrder(orderUpdateRequest);
                    response.Data = updated;
                    response.Message = "Xác nhận hóa đơn (Order) thành công!";
                    response.Success = true;
                }

            } catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Oops! Some thing went wrong.\n" + ex.Message;
                response.Success = false;
            }
            return response;
        }
    }
}
