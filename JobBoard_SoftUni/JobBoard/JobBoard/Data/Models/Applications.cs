using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Applications
    {
        [Key]
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ResumePath { get; set; } = string.Empty;
        public string Status {  get; set; } = string.Empty ;

    }
}
