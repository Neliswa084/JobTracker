using System.ComponentModel.DataAnnotations;

namespace JobTracker.ViewLogic.ViewModels
{
    // This ViewModel is used when the user EDITS an existing job application.
    // It is almost the same as CreateJobApplicationViewModel, but it includes the Id
    // so we know WHICH record in the database to update.
    public class EditJobApplicationViewModel
    {
        // We need the Id to find and update the correct record in the database
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(200)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job title is required")]
        [MaxLength(200)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Job Posting Link (optional)")]
        [DataType(DataType.Url)]
        public string JobLink { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Application Status")]
        public string Status { get; set; } = string.Empty;

        [MaxLength(2000)]
        [Display(Name = "Notes (optional)")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date applied is required")]
        [Display(Name = "Date Applied")]
        [DataType(DataType.Date)]
        public DateTime DateApplied { get; set; }

        // Same status options as the create form
        public List<string> StatusOptions => new List<string>
        {
            "Applied",
            "Interview Scheduled",
            "Interviewed",
            "Technical Test",
            "Offer Received",
            "Rejected",
            "Withdrawn"
        };
    }
}
