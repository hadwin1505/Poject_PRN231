using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IPenaltyRepository : IGenericRepository<Penalty>
    {
        Task<List<Penalty>> GetAllPenalties();
        Task<Penalty> GetPenaltyById(int id);
        Task<List<Penalty>> SearchPenalties(int? schoolId, string? name, string? description, string? status);
        Task<Penalty> CreatePenalty(Penalty penaltyEntity);
        Task<Penalty> UpdatePenalty(Penalty penaltyEntity);
        Task DeletePenalty(int id);
        Task<List<Penalty>> GetPenaltiesBySchoolId(int schoolId);
    }
}
