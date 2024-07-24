using Domain.Entity;
using Domain.Enums.Status;
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
    public class ClassGroupRepository : GenericRepository<ClassGroup>, IClassGroupRepository
    {
        public ClassGroupRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<ClassGroup>> GetAllClassGroups()
        {
            return await _context.ClassGroups
                .Include(c => c.School)
                .ToListAsync();
        }

        public async Task<ClassGroup> GetClassGroupById(int id)
        {
            return await _context.ClassGroups
                .Include(c => c.School)
                .FirstOrDefaultAsync(x => x.ClassGroupId == id);
        }

        public async Task<List<ClassGroup>> SearchClassGroups(int? schoolId, string? hall, int? slot, TimeSpan? time, string? status)
        {
            var query = _context.ClassGroups.AsQueryable();

            if (schoolId.HasValue)
            {
                query = query.Where(p => p.SchoolId == schoolId.Value);
            }
            if (!string.IsNullOrEmpty(hall))
            {
                query = query.Where(p => p.Hall.Contains(hall));
            }
            if (slot.HasValue)
            {
                query = query.Where(p => p.Slot == slot.Value);
            }

            if (time.HasValue)
            {
                query = query.Where(p => p.Time >= time.Value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.Equals(status));
            }

            return await query
                .Include(c => c.School)
                .ToListAsync();
        }

        public async Task<ClassGroup> CreateClassGroup(ClassGroup classGroupEntity)
        {
            await _context.ClassGroups.AddAsync(classGroupEntity);
            await _context.SaveChangesAsync();
            return classGroupEntity;
        }

        public async Task<ClassGroup> UpdateClassGroup(ClassGroup classGroupEntity)
        {
            _context.ClassGroups.Update(classGroupEntity);
            await _context.SaveChangesAsync();
            return classGroupEntity;
        }
        public async Task DeleteClassGroup(int id)
        {
            var classGroupEntity = await _context.ClassGroups.FindAsync(id);
            classGroupEntity.Status = ClassGroupStatusEnums.INACTIVE.ToString();
            _context.Entry(classGroupEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<ClassGroup>> GetClassGroupsBySchoolId(int schoolId)
        {
            return await _context.ClassGroups
                .Include(c => c.School)
                .Where(u => u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
