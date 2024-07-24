using Domain.Entity;
using StudentSupervisorService.Models.Response.OrderResponse;
using StudentSupervisorService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Response.ClassResponse;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using StudentSupervisorService.Models.Request.OrderRequest;
using Domain.Enums.Status;

namespace StudentSupervisorService.Service.Implement
{
    public class OrderImplement : OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<OrderResponse>>> GetAllOrders(string sortOrder)
        {
            var response = new DataResponse<List<OrderResponse>>();
            try
            {
                var orderEntities = await _unitOfWork.Order.GetAllOrders();
                if (orderEntities is null || !orderEntities.Any())
                {
                    response.Message = "Danh sách hóa đơn (Orders) trống!";
                    response.Success = true;
                    return response;
                }

                orderEntities = sortOrder == "desc"
                    ? orderEntities.OrderByDescending(r => r.Total).ToList()
                    : orderEntities.OrderBy(r => r.Total).ToList();

                response.Data = _mapper.Map<List<OrderResponse>>(orderEntities);
                response.Message = "Danh sách hóa đơn (Orders)";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<OrderResponse>> GetOrderById(int orderId)
        {
            var response = new DataResponse<OrderResponse>();
            try
            {
                var orderEntity = await _unitOfWork.Order.GetOrderById(orderId);
                if (orderEntity == null)
                {
                    response.Message = "Không tìm thấy hóa đơn (Order)!";
                    response.Success = false;
                    return response;
                }

                response.Data = _mapper.Map<OrderResponse>(orderEntity);
                response.Message = "Đã tìm thấy hóa đơn (Order)!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<OrderResponse>> GetOrderByOrderCode(int orderCode)
        {
            var response = new DataResponse<OrderResponse>();
            try
            {
                var orderEntity = await _unitOfWork.Order.GetOrderByOrderCode(orderCode);
                if (orderEntity == null)
                {
                    response.Message = "Không tìm thấy hóa đơn (Order)!";
                    response.Success = false;
                    return response;
                }

                response.Data = _mapper.Map<OrderResponse>(orderEntity);
                response.Message = "Đã tìm thấy hóa đơn (Order)!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<List<OrderResponse>>> GetOrdersByUserId(int userId)
        {
            var response = new DataResponse<List<OrderResponse>>();
            try
            {
                var orderEntities = await _unitOfWork.Order.GetOrdersByUserId(userId);
                if (orderEntities is null || !orderEntities.Any())
                {
                    response.Message = "Danh sách hóa đơn (Orders) trống!";
                    response.Success = true;
                    return response;
                }

                response.Data = _mapper.Map<List<OrderResponse>>(orderEntities);
                response.Message = "Danh sách hóa đơn (Orders)";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<List<OrderResponse>>> SearchOrders(
            int? userId, int? packageId, int? orderCode, string? description, int? total, 
            int? amountPaid, int? amountRemaining, string? counterAccountBankName, string? counterAccountNumber, 
            string? counterAccountName, DateTime? date, string? status, string sortOrder)
        {
            var response = new DataResponse<List<OrderResponse>>();

            try
            {
                var orderEntities = await _unitOfWork.Order.SearchOrders(
                    userId, packageId, orderCode, description, total,
                    amountPaid, amountRemaining, counterAccountBankName, counterAccountNumber,
                    counterAccountName, date, status);
                if (orderEntities is null || orderEntities.Count == 0)
                {
                    response.Message = "Không có hóa đơn (Order) nào phù hợp với tiêu chí tìm kiếm !!";
                    response.Success = true;
                }
                else
                {
                    if (sortOrder == "desc")
                    {
                        orderEntities = orderEntities.OrderByDescending(r => r.Total).ToList();
                    }
                    else
                    {
                        orderEntities = orderEntities.OrderBy(r => r.Total).ToList();
                    }
                    response.Data = _mapper.Map<List<OrderResponse>>(orderEntities);
                    response.Message = "Danh sách hóa đơn (Orders)";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
        public async Task<DataResponse<OrderResponse>> CreateOrder(OrderCreateRequest request)
        {
            var response = new DataResponse<OrderResponse>();
            try
            {
                var orderEntity = _mapper.Map<Order>(request);
                orderEntity.Date = DateTime.Now;
                orderEntity.Status = OrderStatusEnum.PENDING.ToString();

                await _unitOfWork.Order.CreateOrder(orderEntity);

                response.Data = _mapper.Map<OrderResponse>(orderEntity);
                response.Message = "Tạo hóa đơn (Order) thành công!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<OrderResponse> UpdateOrder(OrderUpdateRequest request)
        {
            var response = new OrderResponse();
            try
            {
                var existingOrder = await _unitOfWork.Order.GetOrderByOrderCode(request.OrderCode);
                if (existingOrder == null)
                {
                    return response;
                }

                existingOrder.AmountPaid = request.AmountPaid ?? existingOrder.AmountPaid;
                existingOrder.AmountRemaining = request.AmountRemaining ?? existingOrder.AmountRemaining;
                existingOrder.CounterAccountBankName = request.CounterAccountBankName ?? existingOrder.CounterAccountBankName;
                existingOrder.CounterAccountNumber = request.CounterAccountNumber ?? existingOrder.CounterAccountNumber;    
                existingOrder.CounterAccountName = request.CounterAccountName ?? existingOrder.CounterAccountName;
                existingOrder.Status = request.Status ?? existingOrder.Status;

                var updated = await _unitOfWork.Order.UpdateOrder(existingOrder);

                response = _mapper.Map<OrderResponse>(updated);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return response;
        }
        public async Task DeleteOrder(int id)
        {

        }
    }
}
