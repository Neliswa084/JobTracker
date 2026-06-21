using System.ComponentModel.DataAnnotations;

namespace JobTracker.DataLogic.Models
{
    // This class represents a single job application that a user has submitted.
    // Each property maps to a column in the JobApplications table in the database.
    public class JobApplication
    {
        // Primary key - EF Core automatically makes this an auto-incrementing integer
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string JobTitle { get; set; } = string.Empty;

        // Optional - the URL of the job posting
        [MaxLength(500)]
        public string JobLink { get; set; } = string.Empty;

        // Tracks where the application is in the process.
        // Examples: "Applied", "Interview Scheduled", "Interviewed", "Offer Received", "Rejected"
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Applied";

        // Any extra information the user wants to remember about this application
        [MaxLength(2000)]
        public string Notes { get; set; } = string.Empty;

        // The date the user submitted this job application
        public DateTime DateApplied { get; set; }

        // Foreign key - links this job application to the user who created it.
        // This is how we make sure users only see THEIR OWN applications.
        public string UserId { get; set; } = string.Empty;

        // Navigation property - lets us access the full User object from a JobApplication.
        // EF Core uses this to understand the relationship between the two tables.
        public ApplicationUser? User { get; set; }
    }
}
