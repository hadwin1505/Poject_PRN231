using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IEvaluationRepository : IGenericRepository<Evaluation>
    {
        Task<List<Evaluation>> GetAllEvaluations();
        Task<Evaluation> GetEvaluationById(int id);
        Task<List<Evaluation>> SearchEvaluations(int? schoolYearId, int? violationConfigId, string? desciption, DateTime? from, DateTime? to, short? point);
        Task<Evaluation> CreateEvaluation(Evaluation evaluationEntity);
        Task<Evaluation> UpdateEvaluation(Evaluation evaluationEntity);
        Task<List<Evaluation>> GetEvaluationsBySchoolId(int schoolId);
    }
}
