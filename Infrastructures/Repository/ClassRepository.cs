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
    public class ClassRepository : GenericRepository<Class>, IClassReposirory
    {
        public ClassRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<Class>> GetAllClasses()
        {
            return await _context.Classes
                .Include(s => s.SchoolYear)
                .Include(c => c.ClassGroup)
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .ToListAsync();
        }

        public async Task<Class> GetClassById(int id)
        {
            return await _context.Classes
                .Include(s => s.SchoolYear)
                .Include(c => c.ClassGroup)
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(x => x.ClassId == id);
        }

        public async Task<List<Class>> SearchClasses(int? schoolYearId, int? classGroupId, string? code, int? grade, string? name, int? totalPoint)
        {
            var query = _context.Classes.AsQueryable();

            if (schoolYearId != null)
            {
                query = query.Where(p => p.SchoolYearId == schoolYearId);
            }
            if (classGroupId != null)
            {
                query = query.Where(p => p.ClassGroupId == classGroupId);
            }
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(p => p.Code.Contains(code));
            }
            if (grade != null)
            {
                query = query.Where(p => p.Grade == grade);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            if (totalPoint != null)
            {
                query = query.Where(p => p.TotalPoint == totalPoint);
            }

            return await query
                .Include(s => s.SchoolYear)
                .Include(c => c.ClassGroup)
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .ToListAsync();
        }
        public async Task<Class> CreateClass(Class classEntity)
        {
            await _context.Classes.AddAsync(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }
        public async Task<Class> UpdateClass(Class classEntity)
        {
            _context.Classes.Update(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }
        public async Task DeleteClass(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Class>> GetClasssBySchoolId(int schoolId)
        {
            return await _context.Classes
                .Include(c => c.ClassGroup)
                .Include(t => t.Teacher)
                    .ThenInclude(u => u.User)
                .Include(v => v.SchoolYear)
                .Where(v => v.SchoolYear.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
