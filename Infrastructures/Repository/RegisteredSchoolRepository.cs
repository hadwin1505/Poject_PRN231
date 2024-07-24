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
    public class RegisteredSchoolRepository : GenericRepository<RegisteredSchool>, IRegisteredSchoolRepository
    {
        public RegisteredSchoolRepository(SchoolRulesContext context): base(context) { }

        public async Task<List<RegisteredSchool>> GetAllRegisteredSchools()
        {
            return await _context.RegisteredSchools
                .Include(c => c.School)
                .ToListAsync();
        }

        public async Task<RegisteredSchool> GetRegisteredSchoolById(int id)
        {
            return await _context.RegisteredSchools
                .Include(c => c.School)
                .FirstOrDefaultAsync(x => x.RegisteredId == id);
        }

        //get active status registeredschool by school name or school code 
        public async Task<RegisteredSchool> GetActiveSchoolsBySchoolCodeOrName(string? schoolCode, string? schoolName)
        {
            return await _context.RegisteredSchools
                .Where(u => u.School.Code.Equals(schoolCode) || u.School.Name.Equals(schoolName))
                .Where(u => u.Status.Equals(RegisteredSchoolStatusEnums.ACTIVE.ToString()))
                .Include(c => c.School)
                .FirstOrDefaultAsync();
        }

        //get inactive status registeredschool by school name or school code 
        public async Task<RegisteredSchool> GetInactiveSchoolsBySchoolCodeOrName(string? schoolCode, string? schoolName)
        {
            return await _context.RegisteredSchools
                .Where(u => u.School.Code.Equals(schoolCode) || u.School.Name.Equals(schoolName))
                .Where(u => u.Status.Equals(RegisteredSchoolStatusEnums.INACTIVE.ToString()))
                .Include(c => c.School)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RegisteredSchool>> SearchRegisteredSchools(int? schoolId, DateTime? registerdDate, string? description, string? status)
        {
            var query = _context.RegisteredSchools.AsQueryable();

            if (schoolId != null)
            {
                query = query.Where(p => p.SchoolId == schoolId);
            }
            if (registerdDate != null)
            {
                query = query.Where(p => p.RegisteredDate == registerdDate);
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(p => p.Description.Contains(description));
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.Equals(status));
            }
            return await query
                .Include(c => c.School)
                .ToListAsync();
        }

        public async Task<RegisteredSchool> CreateRegisteredSchool(RegisteredSchool registeredSchoolEntity)
        {
            await _context.RegisteredSchools.AddAsync(registeredSchoolEntity);
            await _context.SaveChangesAsync();
            return registeredSchoolEntity;
        }

        public async Task<RegisteredSchool> UpdateRegisteredSchool(RegisteredSchool registeredSchoolEntity)
        {
            _context.RegisteredSchools.Update(registeredSchoolEntity);
            await _context.SaveChangesAsync();
            return registeredSchoolEntity;
        }

        public async Task DeleteRegisteredSchool(int id)
        {
            var registeredSchool = await _context.RegisteredSchools.FindAsync(id);
            registeredSchool.Status = RegisteredSchoolStatusEnums.INACTIVE.ToString();
            _context.Entry(registeredSchool).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<RegisteredSchool>> GetRegisteredSchoolsBySchoolId(int schoolId)
        {
            return await _context.RegisteredSchools
                .Include(c => c.School)
                .Where(u => u.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
