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
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<Package>> GetAllPackages()
        {
            var packages = await _context.Packages
                .Include(x => x.PackageType)
                .ToListAsync();
            return packages;
        }

        public async Task<Package> GetPackageById(int id)
        {
            return _context.Packages
                .Include(x => x.PackageType)
                .FirstOrDefault(v => v.PackageId == id);
        }

        public async Task<List<Package>> SearchPackages(int? packageTypeId, string? name, int? totalStudents, int? totalViolations, int? minPrice, int? maxPrice)
        {
            var query = _context.Packages.AsQueryable();

            if (packageTypeId.HasValue)
            {
                query = query.Where(p => p.PackageTypeId == packageTypeId.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (totalStudents.HasValue)
            {
                query = query.Where(p => p.TotalStudents == totalStudents.Value);
            }

            if (totalViolations.HasValue)
            {
                query = query.Where(p => p.TotalViolations == totalViolations.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }


            return await query
                .Include(x => x.PackageType)
                .ToListAsync();
        }
    }
}
