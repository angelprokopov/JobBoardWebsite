using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage ="")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
