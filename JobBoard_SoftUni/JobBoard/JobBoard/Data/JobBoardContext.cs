using JobBoard.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace JobBoard.Data
{
    public class JobBoardContext : IdentityDbContext
    {   
        public JobBoardContext(DbContextOptions<JobBoardContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Job configuration
            builder.Entity<Job>()
                .HasOne(j => j.Category)
                .WithMany(j => j.Jobs)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasMany(u => u.Applications)
                .WithOne(j => j.Job)
                .HasForeignKey(j => j.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            // User configuration
            builder.Entity<User>()
                .HasMany(u => u.Applications)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            // Company configuration
            builder.Entity<Company>()
                .HasMany(u => u.Jobs)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);


            // JobCategory configuration
            builder.Entity<JobCategory>()
                .HasMany(u => u.Jobs)
                .WithOne(u => u.Category)
                .HasForeignKey(u => u.CategoryId);


            // Application configuration
            builder.Entity<Applications>()
                .Property(a => a.Status)
                .HasDefaultValue("Pending");

            // 
            builder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");
        }

        public DbSet<User> User { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Applications> Applications { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
    }
}
