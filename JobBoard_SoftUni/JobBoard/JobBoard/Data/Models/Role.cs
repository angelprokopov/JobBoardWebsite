using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set;} = new List<UserRole>();
    }

    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
