using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IPatrolScheduleRepository : IGenericRepository<PatrolSchedule>
    {
        Task<List<PatrolSchedule>> GetAllPatrolSchedules();
        Task<PatrolSchedule> GetPatrolScheduleById(int id);
        Task<List<PatrolSchedule>> SearchPatrolSchedules(int? classId, int? supervisorId, int? teacherId, DateTime? from, DateTime? to, string? status);
        Task<PatrolSchedule> CreatePatrolSchedule(PatrolSchedule patrolScheduleEntity);
        Task<PatrolSchedule> UpdatePatrolSchedule(PatrolSchedule patrolScheduleEntity);
        Task DeletePatrolSchedule(int id);
        Task<List<PatrolSchedule>> GetPatrolSchedulesBySchoolId(int schoolId);
    }
}
