using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IHighSchoolRepository : IGenericRepository<HighSchool>
    {
        Task<List<HighSchool>> GetAllHighSchools();
        Task<HighSchool> GetHighSchoolById(int id);
        Task<HighSchool> GetHighSchoolByCodeOrName(string? code, string? name);
        Task<List<HighSchool>> SearchHighSchools(string? code, string? name, string? city, string? address, string? phone);
        Task<HighSchool> CreateHighSchool(HighSchool entity);
    }
}
