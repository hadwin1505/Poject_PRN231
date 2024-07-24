using StudentSupervisorService.Models.Request.DisciplineRequest;
using StudentSupervisorService.Models.Response.DisciplineResponse;
using StudentSupervisorService.Models.Response;


namespace StudentSupervisorService.Service
{
    public interface DisciplineService
    {
        Task<DataResponse<List<DisciplineResponse>>> GetAllDisciplines(string sortOrder);
        Task<DataResponse<DisciplineResponse>> GetDisciplineById(int id);
        Task<DataResponse<List<DisciplineResponse>>> SearchDisciplines(
            int? violationId, 
            int? penaltyId, 
            string? description, 
            DateTime? startDate, 
            DateTime? endDate, 
            string? status, 
            string sortOrder);
        Task<DataResponse<DisciplineResponse>> CreateDiscipline(DisciplineCreateRequest request);
        Task<DataResponse<DisciplineResponse>> UpdateDiscipline(DisciplineUpdateRequest request);
        Task<DataResponse<DisciplineResponse>> DeleteDiscipline(int id);
        Task<DataResponse<List<DisciplineResponse>>> GetDisciplinesBySchoolId(int schoolId);
        Task<DataResponse<DisciplineResponse>> ExecutingDiscipline(int disciplineId);
        Task<DataResponse<DisciplineResponse>> DoneDiscipline(int disciplineId);
    }
}
