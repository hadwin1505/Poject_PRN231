using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ViolationConfigResponse;
using StudentSupervisorService.Models.Request.ViolationConfigRequest;

namespace StudentSupervisorService.Service
{
    public interface ViolationConfigService
    {
        Task<DataResponse<List<ViolationConfigResponse>>> GetAllViolationConfigs(string sortOrder);
        Task<DataResponse<ViolationConfigResponse>> GetViolationConfigById(int id);
        Task<DataResponse<ViolationConfigResponse>> CreateViolationConfig(RequestOfViolationConfig request);
        Task<DataResponse<ViolationConfigResponse>> DeleteViolationConfig(int id);
        Task<DataResponse<ViolationConfigResponse>> UpdateViolationConfig(int id, RequestOfViolationConfig request);
        Task<DataResponse<List<ViolationConfigResponse>>> SearchViolationConfigs(int? vioTypeId, int? minusPoints, string sortOrder);
        Task<DataResponse<List<ViolationConfigResponse>>> GetViolationConfigsBySchoolId(int schoolId);
    }
}
