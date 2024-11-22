using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobApplicationViewModel
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        [Display(Name = "Три имена")]
        public string ApplicantName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Имейл")]
        public string ApplicantEmail { get; set; }

        [Display(Name = "Прикачи CV")]
        public IFormFile Resume { get; set; }
        [Required]
        [Display(Name = "Позиция")]
        public string JobTitle { get; set; }
    }
}
