using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces;
using Infrastructures.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repository
{
    public class EvaluationDetailRepository : GenericRepository<EvaluationDetail>, IEvaluationDetailRepository
    {
        public EvaluationDetailRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<EvaluationDetail>> GetAllEvaluationDetails()
        {
            return await _context.EvaluationDetails
                .Include(c => c.Class)
                .Include(c => c.Evaluation)
                .ToListAsync();
        }

        public async Task<EvaluationDetail> GetEvaluationDetailById(int id)
        {
            return await _context.EvaluationDetails
                .Include(c => c.Class)
                .Include(c => c.Evaluation)
                .FirstOrDefaultAsync(x => x.EvaluationDetailId == id);
        }

        public async Task<List<EvaluationDetail>> SearchEvaluationDetails(int? classId, int? evaluationId, string? status)
        {
            var query = _context.EvaluationDetails.AsQueryable();

            if (classId != null)
            {
                query = query.Where(p => p.ClassId == classId);
            }
            if (evaluationId != null)
            {
                query = query.Where(p => p.EvaluationId == evaluationId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.Equals(status));
            }

            return await query
                .Include(c => c.Class)
                .Include(c => c.Evaluation)
                .ToListAsync();
        }
        public async Task<EvaluationDetail> CreateEvaluationDetail(EvaluationDetail evaluationDetailEntity)
        {
            await _context.EvaluationDetails.AddAsync(evaluationDetailEntity);
            await _context.SaveChangesAsync();
            return evaluationDetailEntity;
        }

        public async Task<EvaluationDetail> UpdateEvaluationDetail(EvaluationDetail evaluationDetailEntity)
        {
            _context.EvaluationDetails.Update(evaluationDetailEntity);
            await _context.SaveChangesAsync();
            return evaluationDetailEntity;
        }

        public async Task DeleteEvaluationDetail(int id)
        {
            var evaluationDetailEntity = await _context.EvaluationDetails.FindAsync(id);
            evaluationDetailEntity.Status = EvaluationDetailStatusEnums.INACTIVE.ToString();
            _context.Entry(evaluationDetailEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<EvaluationDetail>> GetEvaluationDetailsBySchoolId(int schoolId)
        {
            return await _context.EvaluationDetails
                .Include(ed => ed.Evaluation)
                    .ThenInclude(e => e.SchoolYear)
                .Include(ed => ed.Class)
                .Where(ed => ed.Evaluation.SchoolYear.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
