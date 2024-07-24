using Domain.Entity;
using StudentSupervisorService.Models.Request.OrderRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.OrderResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service
{
    public interface OrderService
    {
        Task<DataResponse<List<OrderResponse>>> GetAllOrders(string sortOrder);
        Task<DataResponse<OrderResponse>> GetOrderById(int orderId);
        Task<DataResponse<OrderResponse>> GetOrderByOrderCode(int orderCode);
        Task<DataResponse<List<OrderResponse>>> GetOrdersByUserId(int userId);
        Task<DataResponse<List<OrderResponse>>> SearchOrders(
            int? userId, int? packageId, int? orderCode, string? description, int? total, 
            int? amountPaid, int? amountRemaining, string? counterAccountBankName, 
            string? counterAccountNumber, string? counterAccountName, DateTime? date, string? status, string sortOrder);
        Task<DataResponse<OrderResponse>> CreateOrder(OrderCreateRequest request);
        Task<OrderResponse> UpdateOrder(OrderUpdateRequest request);
        Task DeleteOrder(int id);
    }
}
