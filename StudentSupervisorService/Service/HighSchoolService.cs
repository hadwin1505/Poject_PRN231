using StudentSupervisorService.Models.Request.HighSchoolRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.HighschoolResponse;


namespace StudentSupervisorService.Service
{
    public interface HighSchoolService
    {
        Task<DataResponse<List<ResponseOfHighSchool>>> GetAllHighSchools(string sortOrder);
        Task<DataResponse<ResponseOfHighSchool>> GetHighSchoolById(int id);
        Task<DataResponse<List<ResponseOfHighSchool>>> SearchHighSchools(string? code, string? name, string? city, string? address, string? phone, string sortOrder);
        Task<DataResponse<ResponseOfHighSchool>> CreateHighSchool(RequestOfHighSchool request);
        Task<DataResponse<ResponseOfHighSchool>> DeleteHighSchool(int id);
        Task<DataResponse<ResponseOfHighSchool>> UpdateHighSchool(int id, RequestOfHighSchool request);
    }
}
