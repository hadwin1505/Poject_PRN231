using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.YearPackageResponse;
using StudentSupervisorService.Models.Request.YearPackageRequest;

namespace StudentSupervisorService.Service
{
    public interface YearPackageService
    {
        Task<DataResponse<List<ResponseOfYearPackage>>> GetAllYearPackages(string sortOrder);
        Task<DataResponse<ResponseOfYearPackage>> GetYearPackageById(int id);
        Task<DataResponse<ResponseOfYearPackage>> CreateYearPackage(RequestOfYearPackage request);
        Task<DataResponse<ResponseOfYearPackage>> DeleteYearPackage(int id);
        Task<DataResponse<ResponseOfYearPackage>> UpdateYearPackage(int id, RequestOfYearPackage request);
        Task<DataResponse<List<ResponseOfYearPackage>>> SearchYearPackages(int? schoolYearId, int? packageId, int? minNumberOfStudent, int? maxNumberOfStudent, string sortOrder);
        Task<DataResponse<List<ResponseOfYearPackage>>> GetYearPackagesBySchoolId(int schoolId);
    }
}
