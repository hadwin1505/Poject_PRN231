using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IRegisteredSchoolRepository : IGenericRepository<RegisteredSchool>
    {
        Task<List<RegisteredSchool>> GetAllRegisteredSchools();
        Task<RegisteredSchool> GetRegisteredSchoolById(int id);
        Task<RegisteredSchool> GetActiveSchoolsBySchoolCodeOrName(string? schoolCode, string? schoolName);
        Task<RegisteredSchool> GetInactiveSchoolsBySchoolCodeOrName(string? schoolCode, string? schoolName);
        Task<List<RegisteredSchool>> SearchRegisteredSchools(int? schoolId, DateTime? registerdDate, string? description, string? status);
        Task<RegisteredSchool> CreateRegisteredSchool(RegisteredSchool registeredSchoolEntity);
        Task<RegisteredSchool> UpdateRegisteredSchool(RegisteredSchool registeredSchoolEntity);
        Task DeleteRegisteredSchool(int id);
        Task<List<RegisteredSchool>> GetRegisteredSchoolsBySchoolId(int schoolId);
    }
}
