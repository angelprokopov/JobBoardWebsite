using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobDetailsViewModel
    {
        public Guid JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Salary { get; set; }
        public string Location { get; set; }
        public DateTime PostDate { get; set; }
        public string Category { get; set; }
        public string EmploymentType { get; set; }
        public string ExperienceLevel { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
    }
}
