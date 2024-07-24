using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IYearPackageRepository : IGenericRepository<YearPackage>
    {
        Task<List<YearPackage>> GetAllYearPackages();
        Task<YearPackage> GetYearPackageById(int id);
        Task<List<YearPackage>> SearchYearPackages(int? schoolYearId, int? packageId, int? minNumberOfStudent, int? maxNumberOfStudent);
        Task<List<YearPackage>> GetYearPackagesBySchoolId(int schoolId);
    }
}
