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
    public class PackageTypeRepository : GenericRepository<PackageType>, IPackageTypeRepository
    {
        public PackageTypeRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<PackageType>> GetAllPackageTypes()
        {
            var packageTypes = await _context.PackageTypes.ToListAsync();
            return packageTypes;
        }

        public async Task<PackageType> GetPackageTypeById(int id)
        {
            return _context.PackageTypes.FirstOrDefault();
        }

        public async Task<List<PackageType>> SearchPackageTypes(string? name)
        {
            var query = _context.PackageTypes.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            return await query.ToListAsync();
        }
    }
}
