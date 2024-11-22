using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobDeleteViewModel
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        public string? JobName { get; set; }
        [Required]
        public string? JobTitle { get; set; }
        [Required]
        public string? CompanyName { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }
    }
}
