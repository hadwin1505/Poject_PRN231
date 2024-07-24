using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IPackageTypeRepository : IGenericRepository<PackageType>
    {
        Task<List<PackageType>> GetAllPackageTypes();
        Task<PackageType> GetPackageTypeById(int id);
        Task<List<PackageType>> SearchPackageTypes(string? name);
    }
}
