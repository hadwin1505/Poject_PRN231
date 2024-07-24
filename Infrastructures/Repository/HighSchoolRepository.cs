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
    public class HighSchoolRepository : GenericRepository<HighSchool>, IHighSchoolRepository
    {
        public HighSchoolRepository(SchoolRulesContext context) : base (context) { }
        public async Task<List<HighSchool>> GetAllHighSchools()
        {
            var highSchools = await _context.HighSchools
                .ToListAsync();
            return highSchools;
        }

        public async Task<HighSchool> GetHighSchoolById(int id)
        {
            return await _context.HighSchools.FirstOrDefaultAsync(r => r.SchoolId == id);
        }

        //get highschool by code or name
        public async Task<HighSchool> GetHighSchoolByCodeOrName(string? code, string? name)
        {
            return await _context.HighSchools
                .Where(u => u.Code.Equals(code) || u.Name.Equals(name))
                .FirstOrDefaultAsync();
        }

        public async Task<List<HighSchool>> SearchHighSchools(string? code, string? name, string? city, string? address, string? phone)
        {
            var query = _context.HighSchools.AsQueryable();

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(p => p.Code.Contains(code));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(p => p.City.Contains(city));
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(p => p.Address.Contains(address));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(p => p.Phone.Contains(phone));
            }
            
            return await query.ToListAsync();
        }

        public async Task<HighSchool> CreateHighSchool(HighSchool entity)
        {
            await _context.HighSchools.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
