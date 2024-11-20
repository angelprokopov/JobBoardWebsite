namespace JobBoard.Models
{
    public class JobEditViewModel
    {
       public int Id { get; set; }
        public string? JobName { get; set; }
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public string? Description   { get; set; }
    }
}
