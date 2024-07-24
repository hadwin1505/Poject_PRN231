using Domain.Entity;
using Infrastructures.Interfaces.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IViolationGroupRepository : IGenericRepository<ViolationGroup>
    {
        Task<List<ViolationGroup>> GetAllViolationGroups();
        Task<ViolationGroup> GetViolationGroupById(int id);
        Task<List<ViolationGroup>> SearchViolationGroups(int? schoolId, string? name);
        Task<List<ViolationGroup>> GetViolationGroupBySchoolId(int schoolId);
    }
}
