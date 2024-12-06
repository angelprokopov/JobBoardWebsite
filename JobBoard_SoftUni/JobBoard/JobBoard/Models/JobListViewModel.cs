using JobBoard.Data.Models;

namespace JobBoard.Models
{
    public class JobListViewModel
    {
        public List<Job> Jobs { get; set; } = new List<Job>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public string SelectedCategory { get; set; }
        public IEnumerable<JobCategory> Categories { get; set; }
    }
}
