using StudentSupervisorService.Models.Request.RegisteredSchoolRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.RegisteredSchoolResponse;
namespace StudentSupervisorService.Service
{
    public interface RegisteredSchoolService
    {
        Task<DataResponse<List<RegisteredSchoolResponse>>> GetAllRegisteredSchools(string sortOrder);
        Task<DataResponse<RegisteredSchoolResponse>> GetRegisteredSchoolById(int id);
        Task<DataResponse<List<RegisteredSchoolResponse>>> SearchRegisteredSchools(int? schoolId, DateTime? registerdDate, string? description, string? status, string sortOrder);
        Task<DataResponse<RegisteredSchoolResponse>> CreateRegisteredSchool(RegisteredSchoolCreateRequest registeredSchoolCreateRequest);
        Task<DataResponse<RegisteredSchoolResponse>> UpdateRegisteredSchool(RegisteredSchoolUpdateRequest registeredSchoolUpdateRequest);
        Task<DataResponse<RegisteredSchoolResponse>> DeleteRegisteredSchool(int id);
        Task<DataResponse<List<RegisteredSchoolResponse>>> GetRegisteredSchoolsBySchoolId(int schoolId);
    }
}
