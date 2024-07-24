using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<List<Package>> GetAllPackages();
        Task<Package> GetPackageById(int id);
        Task<List<Package>> SearchPackages(int? packageTypeId ,string? name, int? totalStudents, int? totalViolations ,int? minPrice, int? maxPrice);
    }
}
