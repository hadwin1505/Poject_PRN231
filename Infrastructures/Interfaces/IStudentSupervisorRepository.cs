using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IStudentSupervisorRepository : IGenericRepository<StudentSupervisor>
    {
        Task<List<StudentSupervisor>> GetAllStudentSupervisors();
        Task<StudentSupervisor> GetStudentSupervisorById(int id);
        Task<List<StudentSupervisor>> SearchStudentSupervisors(int? userId, int? studentInClassId);
        Task<List<StudentSupervisor>> GetStudentSupervisorsBySchoolId(int schoolId);
    }
}
