using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PackageResponse;
using StudentSupervisorService.Models.Request.PackageRequest;

namespace StudentSupervisorService.Service
{
    public interface PackageService
    {
        Task<DataResponse<List<ResponseOfPackage>>> GetAllPackages(string sortOrder);
        Task<DataResponse<ResponseOfPackage>> GetPackageById(int id);
        Task<DataResponse<ResponseOfPackage>> CreatePackage(PackageRequest request);
        Task<DataResponse<ResponseOfPackage>> DeletePackage(int id);
        Task<DataResponse<ResponseOfPackage>> UpdatePackage(int id, PackageRequest request);
        Task<DataResponse<List<ResponseOfPackage>>> SearchPackages(int? packageTypeId, string? name, int? totalStudents, int? totalViolations, int? minPrice, int? maxPrice, string sortOrder);
    }
}
