using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ViolationGroupResponse;
using StudentSupervisorService.Models.Request.ViolationGroupRequest;

namespace StudentSupervisorService.Service
{
    public interface ViolationGroupService
    {
        Task<DataResponse<List<ResponseOfVioGroup>>> GetAllVioGroups(string sortOrder);
        Task<DataResponse<ResponseOfVioGroup>> GetVioGroupById(int id);
        Task<DataResponse<ResponseOfVioGroup>> CreateVioGroup(RequestOfVioGroup request);
        Task<DataResponse<ResponseOfVioGroup>> DeleteVioGroup(int id);
        Task<DataResponse<ResponseOfVioGroup>> UpdateVioGroup(int id, RequestOfVioGroup request);
        Task<DataResponse<List<ResponseOfVioGroup>>> SearchVioGroups(int? schoolId, string? name, string sortOrder);
        Task<DataResponse<List<ResponseOfVioGroup>>> GetVioGroupsBySchoolId(int schoolId);
    }
}
