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
    public class EvaluationRepository : GenericRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<Evaluation>> GetAllEvaluations()
        {
            return await _context.Evaluations
                .Include(v => v.ViolationConfig)
                .Include(v => v.SchoolYear)
                .ToListAsync();
        }

        public async Task<Evaluation> GetEvaluationById(int id)
        {
            return await _context.Evaluations
                .Include(v => v.ViolationConfig)
                .Include(v => v.SchoolYear)
                .FirstOrDefaultAsync(x => x.EvaluationId == id);
        }

        public async Task<List<Evaluation>> SearchEvaluations(int? schoolYearId, int? violationConfigID, string? desciption, DateTime? from, DateTime? to, short? point)
        {
            var query = _context.Evaluations.AsQueryable();

            if (schoolYearId != null)
            {
                query = query.Where(p => p.SchoolYearId == schoolYearId);
            }
            if (violationConfigID != null)
            {
                query = query.Where(p => p.ViolationConfigId == violationConfigID);
            }
            if (!string.IsNullOrEmpty(desciption))
            {
                query = query.Where(p => p.Description.Contains(desciption));
            }
            if (from != null)
            {
                query = query.Where(p => p.From >= from);
            }
            if (to != null)
            {
                query = query.Where(p => p.To <= to);
            }
            if (point != null)
            {
                query = query.Where(p => p.Point == point);
            }

            return await query.ToListAsync();
        }
        public async Task<Evaluation> CreateEvaluation(Evaluation evaluationEntity)
        {
            await _context.Evaluations.AddAsync(evaluationEntity);
            await _context.SaveChangesAsync();
            return evaluationEntity;
        }

        public async Task<Evaluation> UpdateEvaluation(Evaluation evaluationEntity)
        {
            _context.Evaluations.Update(evaluationEntity);
            await _context.SaveChangesAsync();
            return evaluationEntity;
        }

        public async Task<List<Evaluation>> GetEvaluationsBySchoolId(int schoolId)
        {
            return await _context.Evaluations
                .Include(v => v.ViolationConfig)
                .Include(v => v.SchoolYear)
                .Where(v => v.SchoolYear.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
