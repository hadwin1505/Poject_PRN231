using StudentSupervisorService.Models.Request.ClassRequest;
using StudentSupervisorService.Models.Response.ClassResponse;
using StudentSupervisorService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSupervisorService.Models.Response.PenaltyResponse;
using StudentSupervisorService.Models.Request.PenaltyRequest;

namespace StudentSupervisorService.Service
{
    public interface PenaltyService
    {
        Task<DataResponse<List<PenaltyResponse>>> GetAllPenalties(string sortOrder);
        Task<DataResponse<PenaltyResponse>> GetPenaltyById(int id);
        Task<DataResponse<List<PenaltyResponse>>> SearchPenalties(int? schoolId, string? name, string? description, string? status, string sortOrder);
        Task<DataResponse<PenaltyResponse>> CreatePenalty(PenaltyCreateRequest penaltyCreateRequest);
        Task<DataResponse<PenaltyResponse>> UpdatePenalty(PenaltyUpdateRequest penaltyUpdateRequest);
        Task<DataResponse<PenaltyResponse>> DeletePenalty(int id);
        Task<DataResponse<List<PenaltyResponse>>> GetPenaltiesBySchoolId(int schoolId);
    }
}
