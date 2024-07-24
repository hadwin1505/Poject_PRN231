using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IEvaluationDetailRepository : IGenericRepository<EvaluationDetail>
    {
        Task<List<EvaluationDetail>> GetAllEvaluationDetails();
        Task<EvaluationDetail> GetEvaluationDetailById(int id);
        Task<List<EvaluationDetail>> SearchEvaluationDetails(int? classId, int? evaluationId, string? status);
        Task<EvaluationDetail> CreateEvaluationDetail(EvaluationDetail evaluationDetailEntity);
        Task<EvaluationDetail> UpdateEvaluationDetail(EvaluationDetail evaluationDetailEntity);
        Task DeleteEvaluationDetail(int id);
        Task<List<EvaluationDetail>> GetEvaluationDetailsBySchoolId(int schoolId);
    }
}
