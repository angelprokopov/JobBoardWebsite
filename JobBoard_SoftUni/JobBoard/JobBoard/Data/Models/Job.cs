using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoard.Data.Models
{
    public class Job
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty ;
        [Required]
        public string Location {  get; set; } = string.Empty ;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public DateTime PostDate { get; set; }
        public string ExperienceLevel { get; set; }
        public string EmploymentType { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid CategoryId { get; set; }
        public JobCategory Category { get; set; }
        public ICollection<Applications> Applications { get; set; } = new List<Applications>();
        public ICollection<Favorite> Favorites { get; set;} = new List<Favorite>();
    }
}
