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
    public class StudentSupervisorRepository : GenericRepository<StudentSupervisor>, IStudentSupervisorRepository
    {
        public StudentSupervisorRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<StudentSupervisor>> GetAllStudentSupervisors()
        {
            var stuSupervisors = await _context.StudentSupervisors
                .Include(c => c.User)
                .Include(s => s.StudentInClass)
                .ToListAsync();
            return stuSupervisors;
        }

        public async Task<StudentSupervisor> GetStudentSupervisorById(int id)
        {
            return _context.StudentSupervisors
              .Include(c => c.User)
              .Include(s => s.StudentInClass)
              .FirstOrDefault(s => s.StudentSupervisorId == id);
        }

        public async Task<List<StudentSupervisor>> GetStudentSupervisorsBySchoolId(int schoolId)
        {
            return await _context.StudentSupervisors
                .Include(v => v.StudentInClass)
                .Include(v => v.User)
                .Where(v => v.User.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<List<StudentSupervisor>> SearchStudentSupervisors(int? userId, int? studentInClassId)
        {
            var query = _context.StudentSupervisors.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }
            if (studentInClassId.HasValue)
            {
                query = query.Where(p => p.StudentInClassId == studentInClassId.Value);
            }

            return await query
                .Include(c => c.User)
                .Include(s => s.StudentInClass)
                .ToListAsync();
        }
    }
}
