namespace JobBoard.Models
{
    public class JobAllViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? CompanyName { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
