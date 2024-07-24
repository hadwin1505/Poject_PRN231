using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.AdminResponse;

namespace StudentSupervisorService.Service
{
    public interface AdminService
    {
        Task<DataResponse<List<AdminResponse>>> GetAllAdmins(string sortOrder);
    }
}
