using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<UserRole> UserRoles { get; set;} = new List<UserRole>();
    }

    public class UserRole : IdentityUserRole<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
