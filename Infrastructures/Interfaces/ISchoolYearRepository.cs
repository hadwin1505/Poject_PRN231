using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface ISchoolYearRepository : IGenericRepository<SchoolYear>
    {
        Task<List<SchoolYear>> GetAllSchoolYears();
        Task<SchoolYear> GetSchoolYearById(int id);
        Task<List<SchoolYear>> SearchSchoolYears(short? year, DateTime? startDate, DateTime? endDate);
        Task<List<SchoolYear>> GetSchoolYearBySchoolId(int schoolId);
        Task<SchoolYear> GetYearBySchoolYearId(int schoolId, short year);
    }
}
