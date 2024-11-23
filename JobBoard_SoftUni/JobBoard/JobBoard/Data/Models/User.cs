using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid GuidId { get; set; } = Guid.NewGuid();
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public ICollection< Applications> Applications { get; set; } = new List<Applications>();  
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
