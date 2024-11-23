using JobBoard.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobFavoriteViewModel
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        [Display(Name = "Позиция")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Фирма")]
        public string Company { get; set; }
        [Required]
        [Display(Name = "Местоположение")]
        public string Location { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }
        public List<JobFavoriteViewModel> Jobs { get; set; } 
    }

    public class JobAllPaginatedViewModel
    {
        public PaginatedList<JobFavoriteViewModel> PaginatedList { get; set; }
    }
}
