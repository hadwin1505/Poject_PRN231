using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.UserResponse;
using StudentSupervisorService.Models.Request.UserRequest;

namespace StudentSupervisorService.Service
{
    public interface UserService
    {
        Task<DataResponse<List<ResponseOfUser>>> GetAllUsers(string sortOrder);
        Task<DataResponse<ResponseOfUser>> GetUserById(int id);
        Task<DataResponse<ResponseOfUser>> CreatePrincipal(RequestOfUser request);
        Task<DataResponse<ResponseOfUser>> CreateSchoolAdmin(RequestOfUser request);
        Task<DataResponse<ResponseOfUser>> DeleteUser(int userId);
        Task<DataResponse<ResponseOfUser>> UpdateUser(int id, RequestOfUser request);
        Task<DataResponse<List<ResponseOfUser>>> SearchUsers(int? schoolId, int? role, string? code, string? name, string? phone, string sortOrder);
        Task<DataResponse<List<ResponseOfUser>>> GetUsersBySchoolId(int schoolId);
    }
}
