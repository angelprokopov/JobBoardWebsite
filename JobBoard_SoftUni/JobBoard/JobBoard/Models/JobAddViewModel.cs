using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobAddViewModel
    {
        [Required]
        public Guid JobId { get; set; }
        [Required]
        [Display(Name = "Позиция")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Местоположене")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Заплата")]
        public decimal Salary { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Категория")]
        public IEnumerable<SelectListItem> Categories  { get; set; }
    }
}
