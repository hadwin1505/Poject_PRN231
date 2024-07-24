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
    public class ViolationTypeRepository : GenericRepository<ViolationType>, IViolationTypeRepository
    {
        public ViolationTypeRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<ViolationType>> GetAllVioTypes()
        {
            var vioTypes = await _context.ViolationTypes
                .Include(v => v.ViolationGroup)
                .ToListAsync();
            return vioTypes;
        }

        public async Task<List<ViolationType>> GetViolationTypesBySchoolId(int schoolId)
        {
            return await _context.ViolationTypes
                .Include(v => v.ViolationGroup)
                .Where(v => v.ViolationGroup.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<ViolationType> GetVioTypeById(int id)
        {
            return _context.ViolationTypes
                .Include(v => v.ViolationGroup)
                .FirstOrDefault(v => v.ViolationTypeId == id);
        }

        public async Task<List<ViolationType>> SearchVioTypes(int? vioGroupId, string? name)
        {
            var query = _context.ViolationTypes.AsQueryable();

            if (vioGroupId.HasValue)
            {
                query = query.Where(p => p.ViolationGroupId == vioGroupId.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            return await query
                .Include(v => v.ViolationGroup)
                .ToListAsync();
        }
    }
}
