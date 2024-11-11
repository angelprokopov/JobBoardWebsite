using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class JobCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
