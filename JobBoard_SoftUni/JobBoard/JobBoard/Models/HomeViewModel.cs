namespace JobBoard.Models
{
    public class HomeViewModel
    {
        public bool IsAuthenticated { get; set; } // Indicates if the user is logged in
        public string Role { get; set; }
    }
}
