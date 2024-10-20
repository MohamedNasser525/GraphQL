using Microsoft.EntityFrameworkCore;
using GraphQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Instructors
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public InstructorsRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<InstructorDTO> GetById(Guid instructorId)
        {
            using (AppDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors.FirstOrDefaultAsync(c => c.Id == instructorId);
            }
        }

        public async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using (AppDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .Where(i => instructorIds.Contains(i.Id))
                    .ToListAsync();
            }
        }
    }
}
