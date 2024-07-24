using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(int orderId);
        Task<Order> GetOrderByOrderCode(int orderCode);
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<List<Order>> SearchOrders(int? userId, int? packageId, int? orderCode, string? description, int? total, int? amountPaid, int? amountRemaining, string? counterAccountBankName, string? counterAccountNumber, string? counterAccountName, DateTime? date, string? status);
        Task<Order> CreateOrder(Order orderEntity);
        Task<Order> UpdateOrder(Order orderEntity);
        Task DeleteOrder(int id);
    }
}
