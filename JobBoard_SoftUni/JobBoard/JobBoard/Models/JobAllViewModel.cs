using JobBoard.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobAllViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Позиция")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        public decimal Salary { get; set; }
        [Required]
        [Display(Name = "Местоположение")]
        public string? Location { get; set; }
        [Required]
        [Display(Name = "Име на компанията")]
        public string? CompanyName { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
