using StudentSupervisorService.Models.Request.ClassRequest;
using StudentSupervisorService.Models.Response.ClassResponse;
using StudentSupervisorService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSupervisorService.Models.Response.ClassGroupResponse;
using StudentSupervisorService.Models.Request.ClassGroupRequest;

namespace StudentSupervisorService.Service
{
    public interface ClassGroupService
    {
        Task<DataResponse<List<ClassGroupResponse>>> GetAllClassGroups(string sortOrder);
        Task<DataResponse<ClassGroupResponse>> GetClassGroupById(int id);
        Task<DataResponse<List<ClassGroupResponse>>> SearchClassGroups(int? schoolId, string? hall, int? slot, TimeSpan? time, string? status, string sortOrder);
        Task<DataResponse<ClassGroupResponse>> CreateClassGroup(ClassGroupCreateRequest classGroupCreateRequest);
        Task<DataResponse<ClassGroupResponse>> UpdateClassGroup(ClassGroupUpdateRequest classGroupUpdateRequest);
        Task<DataResponse<ClassGroupResponse>> DeleteClassGroup(int id);
        Task<DataResponse<List<ClassGroupResponse>>> GetClassGroupsBySchoolId(int schoolId);
    }
}
