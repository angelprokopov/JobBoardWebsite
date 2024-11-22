using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobDetailsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }
        [Required]
        public List<string> Requirements { get; set; } = new List<string>();
        public List<string> Responsibilities { get; set; } = new List<string>();
    }
}
