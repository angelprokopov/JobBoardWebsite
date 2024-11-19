﻿namespace JobBoard.Models
{
    public class JobDeleteViewModel
    {
        public int JobId { get; set; }
        public string? JobName { get; set; }
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
