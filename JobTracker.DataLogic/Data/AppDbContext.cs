using JobTracker.DataLogic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.DataLogic.Data
{
    // AppDbContext is our "gateway" to the database.
    // It inherits from IdentityDbContext<ApplicationUser> so it also automatically
    // creates all the Identity tables (users, roles, logins, etc.) for us.
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // The constructor receives options (like the connection string) from Program.cs
        // and passes them up to the base class.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet represents a table in the database.
        // This gives us a JobApplications table we can query with LINQ.
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Always call base first when using Identity - it sets up all the Identity tables
            base.OnModelCreating(builder);

            // Configure the JobApplication table
            builder.Entity<JobApplication>(entity =>
            {
                // CompanyName is required and has a max length
                entity.Property(e => e.CompanyName)
                      .IsRequired()
                      .HasMaxLength(200);

                // JobTitle is required
                entity.Property(e => e.JobTitle)
                      .IsRequired()
                      .HasMaxLength(200);

                // Status is required
                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(50);

                // JobLink and Notes are optional (can be empty)
                entity.Property(e => e.JobLink).HasMaxLength(500);
                entity.Property(e => e.Notes).HasMaxLength(2000);

                // UserId links each job application to the user who created it
                entity.Property(e => e.UserId).IsRequired();

                // Set up the relationship: one ApplicationUser has many JobApplications
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, delete their applications too
            });
        }
    }
}
