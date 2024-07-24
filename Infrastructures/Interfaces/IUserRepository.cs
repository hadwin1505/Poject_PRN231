using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetAccountByPhone(string phone);
        Task<List<User>> SearchUsers(int? schoolId, int? role, string? code, string? name, string? phone);
        Task<List<User>> GetUsersBySchoolId(int schoolId);
    }
}
