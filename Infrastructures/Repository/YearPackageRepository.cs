using Domain.Entity;
using Infrastructures.Interfaces;
using Infrastructures.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructures.Repository
{
    public class YearPackageRepository : GenericRepository<YearPackage>, IYearPackageRepository
    {
        public YearPackageRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<YearPackage>> GetAllYearPackages()
        {
            var yearPackage = await _context.YearPackages
                .Include(s => s.SchoolYear)
                .Include(s => s.Package)
                .ToListAsync();
            return yearPackage;
        }

        public async Task<YearPackage> GetYearPackageById(int id)
        {
            return _context.YearPackages
                .Include(s => s.SchoolYear)
                .Include(s => s.Package)
                .FirstOrDefault();
        }

        public async Task<List<YearPackage>> GetYearPackagesBySchoolId(int schoolId)
        {
            return await _context.YearPackages
                .Include(v => v.Package)
                .Include(v => v.SchoolYear)
                .Where(v => v.SchoolYear.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<List<YearPackage>> SearchYearPackages(int? schoolYearId, int? packageId, int? minNumberOfStudent, int? maxNumberOfStudent)
        {
            var query = _context.YearPackages.AsQueryable();

            if (schoolYearId.HasValue)
            {
                query = query.Where(p => p.SchoolYearId == schoolYearId.Value);
            }

            if (packageId.HasValue)
            {
                query = query.Where(p => p.PackageId == packageId.Value);
            }

            if (minNumberOfStudent.HasValue)
            {
                query = query.Where(p => p.NumberOfStudent >= minNumberOfStudent.Value);
            }

            if (maxNumberOfStudent.HasValue)
            {
                query = query.Where(p => p.NumberOfStudent <= maxNumberOfStudent.Value);
            }

            return await query
                .Include(s => s.SchoolYear)
                .Include(s => s.Package)
                .ToListAsync();
        }
    }
}
