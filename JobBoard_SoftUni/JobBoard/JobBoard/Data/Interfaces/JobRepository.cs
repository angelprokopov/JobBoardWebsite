using JobBoard.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Data.Interfaces
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(JobBoardContext context) : base(context) { }

        public async Task<IEnumerable<Job>> GetJobsWithCategoryAndCompanyAsync()
        {
            return await _context.Set<Job>()
                .Include(j=>j.Category)
                .Include(j=>j.Company)
                .ToListAsync();
        }
    }
}
