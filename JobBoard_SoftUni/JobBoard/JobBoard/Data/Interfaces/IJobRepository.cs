using JobBoard.Data.Models;

namespace JobBoard.Data.Interfaces
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<IEnumerable<Job>> GetJobsWithCategoryAndCompanyAsync();
    }
}
