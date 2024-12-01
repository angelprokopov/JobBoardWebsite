using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty ;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
