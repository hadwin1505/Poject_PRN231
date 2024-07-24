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
    public class StudentInClassRepository : GenericRepository<StudentInClass>, IStudentInClassRepository
    {
        public StudentInClassRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<StudentInClass>> GetAllStudentInClass()
        {
            return await _context.StudentInClasses
                .Include(v => v.Class)
                .Include(s => s.Student)
                .ToListAsync();
        }

        public async Task<StudentInClass> GetStudentInClassById(int id)
        {
            return await _context.StudentInClasses
                .Include(v => v.Class)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(x => x.StudentInClassId == id);
        }

        public async Task<List<StudentInClass>> SearchStudentInClass(int? classId, int? studentId, DateTime? enrollDate, bool? isSupervisor, DateTime? startDate, DateTime? endDate, int? numberOfViolation, string? status)
        {
            var query = _context.StudentInClasses.AsQueryable();

            if (classId != null)
            {
                query = query.Where(p => p.ClassId == classId);
            }
            if (studentId != null)
            {
                query = query.Where(p => p.StudentId == studentId);
            }
            if (enrollDate != null)
            {
                query = query.Where(p => p.EnrollDate == enrollDate);
            }
            if (isSupervisor != null)
            {
                query = query.Where(p => p.IsSupervisor == isSupervisor);
            }
            if (startDate != null)
            {
                query = query.Where(p => p.StartDate == startDate);
            }
            if (endDate != null)
            {
                query = query.Where(p => p.EndDate == endDate);
            }
            if (numberOfViolation != null)
            {
                query = query.Where(p => p.NumberOfViolation == numberOfViolation);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.Contains(status));
            }

            return await query
                .Include(v => v.Class)
                .Include(s => s.Student)
                .ToListAsync();
        }
        public async Task<StudentInClass> CreateStudentInClass(StudentInClass studentInClassEntity)
        {
            await _context.StudentInClasses.AddAsync(studentInClassEntity);
            await _context.SaveChangesAsync();
            return studentInClassEntity;
        }

        public async Task<StudentInClass> UpdateStudentInClass(StudentInClass studentInClassEntity)
        {
            _context.StudentInClasses.Update(studentInClassEntity);
            await _context.SaveChangesAsync();
            return studentInClassEntity;
        }

        public async Task DeleteStudentInClass(int id)
        {
            var studentInClassEntity = await _context.StudentInClasses.FindAsync(id);
            studentInClassEntity.Status = StudentInClassStatusEnums.UNENROLLED.ToString();
            _context.Entry(studentInClassEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsStudentEnrolledInAnyClass(int studentId)
        {
            return await _context.StudentInClasses.AnyAsync(sic => sic.StudentId == studentId && sic.Status == StudentInClassStatusEnums.ENROLLED.ToString());
        }

        public async Task<List<StudentInClass>> GetStudentInClassesBySchoolId(int schoolId)
        {
            return await _context.StudentInClasses
                .Include(v => v.Class)
                .Include(v => v.Student)
                .Where(v => v.Student.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
