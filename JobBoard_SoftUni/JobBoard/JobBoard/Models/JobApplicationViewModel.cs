using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobApplicationViewModel
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string ApplicantName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string ApplicantEmail { get; set; }

        [Display(Name = "Upload Resume")]
        public IFormFile Resume { get; set; }

        public string JobTitle { get; set; }
    }
}
