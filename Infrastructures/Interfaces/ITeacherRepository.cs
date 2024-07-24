using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<List<Teacher>> GetAllTeachers();
        Task<Teacher> GetTeacherById(int id);
        Task<Teacher> GetTeacherByUserId(int id);
        Task<List<Teacher>> SearchTeachers(int? schoolId, int? userId, bool sex);
        Task<Teacher> GetTeacherByIdWithUser(int id);
        Task<List<Teacher>> GetTeachersBySchoolId(int schoolId);
    }
}
