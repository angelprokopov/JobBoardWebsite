using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        public int UserId { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
