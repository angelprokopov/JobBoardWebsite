using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Favorite
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; }
    }
}
