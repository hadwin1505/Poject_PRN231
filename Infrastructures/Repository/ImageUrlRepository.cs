using Domain.Entity;
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
    public class ImageUrlRepository : GenericRepository<ImageUrl>, IImageUrlRepository
    {
        public ImageUrlRepository(SchoolRulesContext context) : base(context) { }

        public async Task<List<ImageUrl>> GetImageUrlsBySchoolId(int schoolId)
        {
            return await _context.ImageUrls
                .Include(ed => ed.Violation)
                    .ThenInclude(e => e.Teacher)
                .Where(ed => ed.Violation.Teacher.SchoolId == schoolId)
                .ToListAsync();
        }
    }
}
