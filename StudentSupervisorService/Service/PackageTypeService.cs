using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PackageTypeResponse;
using StudentSupervisorService.Models.Request.PackageTypeRequest;

namespace StudentSupervisorService.Service
{
    public interface PackageTypeService 
    {
        Task<DataResponse<List<PackageTypeResponse>>> GetAllPackageTypes(string sortOrder);
        Task<DataResponse<PackageTypeResponse>> GetPackageTypeById(int id);
        Task<DataResponse<PackageTypeResponse>> CreatePackageType(PackageTypeRequest request);
        Task<DataResponse<PackageTypeResponse>> DeletePackageType(int id);
        Task<DataResponse<PackageTypeResponse>> UpdatePackageType(int id, PackageTypeRequest request);
        Task<DataResponse<List<PackageTypeResponse>>> SearchPackageTypes(string? name, string sortOrder);
    }
}
