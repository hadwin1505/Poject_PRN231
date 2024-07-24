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
    public class ViolationGroupRepository : GenericRepository<ViolationGroup>, IViolationGroupRepository
    {
        public ViolationGroupRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<ViolationGroup>> GetAllViolationGroups()
        {
            var vioGroups = await _context.ViolationGroups
                .Include(v => v.School)
                .ToListAsync();
            return vioGroups;
        }

        public async Task<ViolationGroup> GetViolationGroupById(int id)
        {
            return _context.ViolationGroups
                .Include(v => v.School)
                .FirstOrDefault(v => v.ViolationGroupId == id);
        }

        public async Task<List<ViolationGroup>> GetViolationGroupBySchoolId(int schoolId)
        {
            return await _context.ViolationGroups
                .Include(c => c.School)
                .Where(u => u.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<List<ViolationGroup>> SearchViolationGroups(int? schoolId, string? name)
        {
            var query = _context.ViolationGroups.AsQueryable();

            if (schoolId.HasValue)
            {
                query = query.Where(p => p.SchoolId == schoolId.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            return await query
                .Include(v => v.School)
                .ToListAsync();
        }
    }
}
