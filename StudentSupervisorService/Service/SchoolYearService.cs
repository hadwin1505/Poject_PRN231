using StudentSupervisorService.Models.Request.SchoolYearRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.SchoolYearResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service
{
    public interface SchoolYearService
    {
        Task<DataResponse<List<ResponseOfSchoolYear>>> GetAllSchoolYears(string sortOrder);
        Task<DataResponse<ResponseOfSchoolYear>> GetSchoolYearById(int id);
        Task<DataResponse<ResponseOfSchoolYear>> CreateSchoolYear(RequestCreateSchoolYear request);
        Task<DataResponse<ResponseOfSchoolYear>> DeleteSchoolYear(int id);
        Task<DataResponse<ResponseOfSchoolYear>> UpdateSchoolYear(int id, RequestCreateSchoolYear request);
        Task<DataResponse<List<ResponseOfSchoolYear>>> SearchSchoolYears(short? year, DateTime? startDate, DateTime? endDate, string sortOrder);
        Task<DataResponse<List<ResponseOfSchoolYear>>> GetSchoolYearBySchoolId(int schoolId);
    }
}
