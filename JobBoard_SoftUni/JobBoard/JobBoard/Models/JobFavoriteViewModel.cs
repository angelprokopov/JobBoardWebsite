    using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobFavoriteViewModel
    {
        [Required]
        public int Id { get; set; }
        public SelectList Jobs { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        [Required]
        [Display(Name = "Позиция")]
        public string? JobTitle { get; set; }
        [Required]
        [Display(Name = "Име на фирмата")]
        public string? CompanyName { get; set; }
        [Required]
        [Display(Name = "Местоположение")]
        public string? Location { get; set; }
        public int UserId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
