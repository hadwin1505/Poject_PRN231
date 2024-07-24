using Domain.Entity;
using Infrastructures.Interfaces;
using Infrastructures.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repository
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students
                .Include(s => s.School)
                .ToListAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _context.Students
                .Include(s => s.School)
                .FirstOrDefaultAsync(x => x.StudentId == id);
        }

        public async Task<List<Student>> SearchStudents(int? schoolId, string? code, string? name, bool? sex, DateTime? birthday, string? address, string? phone)
        {
            var query = _context.Students.AsQueryable();

            if (schoolId != null)
            {
                query = query.Where(p => p.SchoolId == schoolId);
            }
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(p => p.Code.Contains(code));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            if (sex != null)
            {
                query = query.Where(p => p.Sex.Equals(sex));
            }
            if (birthday != null)
            {
                query = query.Where(p => p.Birthday == birthday);
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(p => p.Address.Contains(address));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(p => p.Phone.Contains(phone));
            }
            return await query
                .Include(s => s.School)
                .ToListAsync();
        }
        public async Task<Student> CreateStudent(Student studentEntity)
        {
            await _context.Students.AddAsync(studentEntity);
            await _context.SaveChangesAsync();
            return studentEntity;
        }

        public async Task<Student> UpdateStudent(Student studentEntity)
        {
            _context.Students.Update(studentEntity);
            await _context.SaveChangesAsync();
            return studentEntity;
        }

        public async Task DeleteStudent(int id)
        {
            try
            {
                var studentEntity = await _context.Students.FindAsync(id);
                if (studentEntity != null)
                {
                    _context.Students.Remove(studentEntity);
                    await _context.SaveChangesAsync();
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        public async Task<Student> GetStudentByCode(string code)
        {
            return _context.Students
                .Include(s => s.School)
                .FirstOrDefault(x => x.Code == code);
        }

        public async Task<List<Student>> GetStudentsBySchoolId(int schoolId)
        {
            return await _context.Students
                .Include(c => c.School)
                .Where(u => u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
