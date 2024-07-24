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
    public class SchoolYearRepository : GenericRepository<SchoolYear>, ISchoolYearRepository
    {
        public SchoolYearRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<SchoolYear>> GetAllSchoolYears()
        {
            var schoolYears = await _context.SchoolYears
                .Include(c => c.School)
                .ToListAsync();
            return schoolYears;
        }

        public async Task<SchoolYear> GetSchoolYearById(int id)
        {
            return _context.SchoolYears
               .Include(c => c.School)
               .FirstOrDefault(s => s.SchoolYearId == id);
        }

        public async Task<List<SchoolYear>> GetSchoolYearBySchoolId(int schoolId)
        {
            return await _context.SchoolYears
                .Include(c => c.School)
                .Where(m => m.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<SchoolYear> GetYearBySchoolYearId(int schoolId, short year)
        {
            return _context.SchoolYears
                .Include(c => c.School)
                .FirstOrDefault(s => s.SchoolId == schoolId && s.Year == year);
        }

        public async Task<List<SchoolYear>> SearchSchoolYears(short? year, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.SchoolYears.AsQueryable();

            if (year.HasValue)
            {
                query = query.Where(p => p.Year == year.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= endDate.Value);
            }

            return await query.Include(c => c.School).ToListAsync();
        }
    }
}
