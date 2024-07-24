using StudentSupervisorService.Models.Request.StudentRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.StudentResponse;


namespace StudentSupervisorService.Service
{
    public interface StudentService
    {
        Task<DataResponse<List<StudentResponse>>> GetAllStudents(string sortOrder);
        Task<DataResponse<StudentResponse>> GetStudentById(int id);
        Task<DataResponse<List<StudentResponse>>> SearchStudents(
            int? schoolId, 
            string? code, 
            string? name, 
            bool? sex, 
            DateTime? birthday, 
            string? address, 
            string? phone,
            string sortOrder);
        Task<DataResponse<StudentResponse>> CreateStudent(StudentCreateRequest studentCreateRequest);
        Task<DataResponse<StudentResponse>> UpdateStudent(StudentUpdateRequest studentUpdateRequest);
        Task<DataResponse<StudentResponse>> DeleteStudent(int studentId);
        Task<DataResponse<List<StudentResponse>>> GetStudentsBySchoolId(int schoolId);
    }
}
