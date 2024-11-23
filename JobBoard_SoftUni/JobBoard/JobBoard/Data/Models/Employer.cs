﻿using System.ComponentModel.DataAnnotations;

namespace JobBoard.Data.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int CompanyId { get; set; }  
        public Company Company { get; set; }
    }
}
