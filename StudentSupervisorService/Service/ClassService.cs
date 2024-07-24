using StudentSupervisorService.Models.Request.ClassRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ClassResponse;


namespace StudentSupervisorService.Service
{
    public interface ClassService
    {
        Task<DataResponse<List<ClassResponse>>> GetAllClasses(string sortOrder);
        Task<DataResponse<ClassResponse>> GetClassById(int id);
        Task<DataResponse<List<ClassResponse>>> SearchClasses(int? schoolYearId, int? classGroupId, string? code, int? grade, string? name, int? totalPoint, string sortOrder);
        Task<DataResponse<ClassResponse>> CreateClass(ClassCreateRequest classCreateRequest);
        Task<DataResponse<ClassResponse>> UpdateClass(ClassUpdateRequest classUpdateRequest);
        Task<DataResponse<ClassResponse>> DeleteClass(int id);
        Task<DataResponse<List<ClassResponse>>> GetClassesBySchoolId(int schoolId);
    }
}
