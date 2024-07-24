using Domain.Entity;
using Infrastructures.Interfaces;
using Infrastructures.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(s => s.Package)
                .Include(c => c.User)
                .ToListAsync();
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(s => s.Package)
                .Include(c => c.User)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
        }
        public async Task<Order> GetOrderByOrderCode(int orderCode)
        {
            return await _context.Orders
                .Include(s => s.Package)
                .Include(c => c.User)
                .FirstOrDefaultAsync(x => x.OrderCode == orderCode);
        }
        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Include(s => s.Package)
                .Include(c => c.User)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<Order>> SearchOrders(int? userId, int? packageId, int? orderCode, string? description, int? total, int? amountPaid, int? amountRemaining, string? counterAccountBankName, string? counterAccountNumber, string? counterAccountName, DateTime? date, string? status)
        {
            var query = _context.Orders.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }
            if (packageId.HasValue)
            {
                query = query.Where(p => p.PackageId == packageId.Value);
            }
            if (orderCode.HasValue)
            {
                query = query.Where(p => p.OrderCode == orderCode.Value);
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(p => p.Description.Contains(description));
            }
            if (total.HasValue)
            {
                query = query.Where(p => p.Total == total.Value);
            }
            if (amountPaid.HasValue)
            {
                query = query.Where(p => p.AmountPaid == amountPaid.Value);
            }
            if (amountRemaining.HasValue)
            {
                query = query.Where(p => p.AmountRemaining == amountRemaining.Value);
            }
            if (!string.IsNullOrEmpty(counterAccountBankName))
            {
                query = query.Where(p => p.CounterAccountBankName.Contains(counterAccountBankName));
            }
            if (!string.IsNullOrEmpty(counterAccountNumber))
            {
                query = query.Where(p => p.CounterAccountNumber.Contains(counterAccountNumber));
            }
            if (!string.IsNullOrEmpty(counterAccountName))
            {
                query = query.Where(p => p.CounterAccountName.Contains(counterAccountName));
            }
            if (date.HasValue)
            {
                query = query.Where(p => p.Date == date.Value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.Equals(status));
            }

            return await query
                .Include(s => s.Package)
                .Include(c => c.User)
                .ToListAsync();
        }
        public async Task<Order> CreateOrder(Order orderEntity)
        {
            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
            return orderEntity;
        }
        public async Task<Order> UpdateOrder(Order orderEntity)
        {
            _context.Orders.Update(orderEntity);
            await _context.SaveChangesAsync();
            return orderEntity;
        }
        public async Task DeleteOrder(int id)
        {

        }
    }
}
