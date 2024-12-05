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

        public List<JobCategory> Categories { get; set; }
        public List<Job> Jobs { get; set; }
        public Guid SelectedJobId { get; set; }
    }

    public class JobAllPaginatedViewModel
    {
        public PaginatedList<JobAllViewModel> PaginatedList { get; set; }
    }
}
