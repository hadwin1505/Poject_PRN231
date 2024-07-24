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
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(SchoolRulesContext context) : base(context) { }

        public async Task<Admin> GetAccountByPhone(string phone)
        {
            return _context.Admins
            .Include(a => a.Role)
            .FirstOrDefault(a => a.Phone == phone);
        }

        public async Task<List<Admin>> GetAllAdmins()
        {
            var admins = await _context.Admins
                .Include(c => c.Role)
                .ToListAsync();
            return admins;
        }
    }
}
