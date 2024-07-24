using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<Admin> GetAccountByPhone(string phone);
        Task<List<Admin>> GetAllAdmins();
    }
}
