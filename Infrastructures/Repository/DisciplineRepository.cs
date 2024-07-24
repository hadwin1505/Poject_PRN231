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
    public class DisciplineRepository : GenericRepository<Discipline>, IDisciplineRepository
    {
        public DisciplineRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<Discipline>> GetAllDisciplines()
        {
            return await _context.Disciplines
                .Include(v => v.Violation)
                    .ThenInclude(c => c.StudentInClass)
                    .ThenInclude(c => c.Student)
                .Include(v => v.Pennalty)
                .ToListAsync();
        }

        public async Task<Discipline> GetDisciplineById(int id)
        {
            return await _context.Disciplines
                .Include(v => v.Violation)
                    .ThenInclude(c => c.StudentInClass)
                    .ThenInclude(c => c.Student)
                .Include(v => v.Pennalty)
                .FirstOrDefaultAsync(x => x.DisciplineId == id);
        }

        public async Task<List<Discipline>> SearchDisciplines(int? violationId, int? penaltyId, string? description, DateTime? startDate, DateTime? endDate, string? status)
        {
            var query = _context.Disciplines.AsQueryable();

            if (violationId != null)
            {
                query = query.Where(p => p.ViolationId == violationId);
            }
            if (penaltyId != null)
            {
                query = query.Where(p => p.PennaltyId == penaltyId);
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(p => p.Description.Contains(description));
            }
            if (startDate != null)
            {
                query = query.Where(p => p.StartDate >= startDate);
            }
            if (endDate != null)
            {
                query = query.Where(p => p.EndDate <= endDate);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.Equals(status));
            }

            return await query
                .Include(v => v.Violation)
                    .ThenInclude(c => c.StudentInClass)
                    .ThenInclude(c => c.Student)
                .Include(v => v.Pennalty)
                .ToListAsync();
        }

        public async Task<Discipline> CreateDiscipline(Discipline disciplineEntity)
        {
            await _context.Disciplines.AddAsync(disciplineEntity);
            await _context.SaveChangesAsync();
            return disciplineEntity;
        }

        public async Task<Discipline> UpdateDiscipline(Discipline disciplineEntity)
        {
            _context.Disciplines.Update(disciplineEntity);
            await _context.SaveChangesAsync();
            return disciplineEntity;
        }

        public async Task DeleteDiscipline(int id)
        {
            var disciplineEntity = await _context.Disciplines.FindAsync(id);
            disciplineEntity.Status = DisciplineStatusEnums.INACTIVE.ToString();
            _context.Entry(disciplineEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Discipline>> GetDisciplinesBySchoolId(int schoolId)
        {
            return await _context.Disciplines
                .Include(v => v.Violation)
                    .ThenInclude(c => c.StudentInClass)
                    .ThenInclude(c => c.Student)
                .Include(v => v.Pennalty)
                .Where(v => v.Pennalty.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<Discipline> GetDisciplineByViolationId(int violationId)
        {
            return await _context.Disciplines
                .Include(v => v.Violation)
                    .ThenInclude(c => c.StudentInClass)
                    .ThenInclude(c => c.Student)
                .Include(v => v.Pennalty)
                .FirstOrDefaultAsync(x => x.ViolationId == violationId);
        }
    }
}
