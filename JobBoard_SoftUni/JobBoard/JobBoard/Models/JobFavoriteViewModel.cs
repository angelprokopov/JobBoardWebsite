using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobBoard.Models
{
    public class JobFavoriteViewModel
    {
        public int Id { get; set; }
        public SelectList Jobs { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>()); 
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public int UserId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
