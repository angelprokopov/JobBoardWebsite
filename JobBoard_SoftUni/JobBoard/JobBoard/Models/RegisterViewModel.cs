using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }  
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm passwrod")]
        public string ConfirmPassword { get; set; }
    }
}
