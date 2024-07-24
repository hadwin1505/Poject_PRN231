using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces;
using Infrastructures.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repository
{
    public class PatrolScheduleRepository : GenericRepository<PatrolSchedule>, IPatrolScheduleRepository
    {
        public PatrolScheduleRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<PatrolSchedule>> GetAllPatrolSchedules()
        {
            return await _context.PatrolSchedules       
                .Include(v => v.Class)
                .Include(p => p.Supervisor)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .ToListAsync();
        }

        public async Task<PatrolSchedule> GetPatrolScheduleById(int id)
        {
            return await _context.PatrolSchedules
                .Include(v => v.Class)
                .Include(p => p.Supervisor)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(x => x.ScheduleId == id);
        }

        public async Task<List<PatrolSchedule>> SearchPatrolSchedules(int? classId, int? supervisorId, int? teacherId, DateTime? from, DateTime? to, string? status)
        {
            var query = _context.PatrolSchedules.AsQueryable();

            if (classId != null)
            {
                query = query.Where(p => p.ClassId == classId);
            }
            if (supervisorId != null)
            {
                query = query.Where(p => p.SupervisorId == supervisorId);
            }
            if (teacherId != null)
            {
                query = query.Where(p => p.TeacherId == teacherId);
            }
            if (from != null)
            {
                query = query.Where(p => p.From >= from);
            }
            if (to != null)
            {
                query = query.Where(p => p.To <= to);
            }
            if (!status.IsNullOrEmpty())
            {
                query = query.Where(p => p.Status.Equals(status));
            }

            return await query
                .Include(v => v.Class)
                .Include(p => p.Supervisor)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .ToListAsync();
        }
        public async Task<PatrolSchedule> CreatePatrolSchedule(PatrolSchedule patrolScheduleEntity)
        {
            await _context.PatrolSchedules.AddAsync(patrolScheduleEntity);
            await _context.SaveChangesAsync();
            return patrolScheduleEntity;
        }

        public async Task<PatrolSchedule> UpdatePatrolSchedule(PatrolSchedule patrolScheduleEntity)
        {
            _context.PatrolSchedules.Update(patrolScheduleEntity);
            _context.Entry(patrolScheduleEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return patrolScheduleEntity;
        }

        public async Task DeletePatrolSchedule(int id)
        {
            try
            {
                var pScheduleEntity = await _context.PatrolSchedules.FindAsync(id);
                pScheduleEntity.Status = PatrolScheduleStatusEnums.INACTIVE.ToString();
                _context.Entry(pScheduleEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        public async Task<List<PatrolSchedule>> GetPatrolSchedulesBySchoolId(int schoolId)
        {
            return await _context.PatrolSchedules
                .Include(v => v.Class)
                .Include(p => p.Supervisor)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Where(v => v.Teacher.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
