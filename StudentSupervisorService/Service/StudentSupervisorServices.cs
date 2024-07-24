using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.StudentSupervisorResponse;
using StudentSupervisorService.Models.Request.StudentSupervisorRequest;

namespace StudentSupervisorService.Service
{
    public interface StudentSupervisorServices
    {
        Task<DataResponse<List<StudentSupervisorResponse>>> GetAllStudentSupervisors(string sortOrder);
        Task<DataResponse<StudentSupervisorResponse>> GetStudentSupervisorById(int id);
        Task<DataResponse<StudentSupervisorResponse>> CreateAccountStudentSupervisor(StudentSupervisorRequest request);
        Task<DataResponse<StudentSupervisorResponse>> DeleteStudentSupervisor(int id);
        Task<DataResponse<StudentSupervisorResponse>> UpdateStudentSupervisor(int id, StudentSupervisorRequest request);
        Task<DataResponse<List<StudentSupervisorResponse>>> SearchStudentSupervisors(int? userId, int? studentInClassId, string sortOrder);
        Task<DataResponse<List<StudentSupervisorResponse>>> GetStudentSupervisorsBySchoolId(int schoolId);
    }
}
