using System.ComponentModel.DataAnnotations;

namespace JobTracker.ViewLogic.ViewModels
{
    // This ViewModel is used to DISPLAY a job application to the user (e.g. on the list or details page).
    // A ViewModel is different from a Model - it's shaped specifically for what the VIEW needs to show,
    // not exactly how data is stored in the database.
    public class JobApplicationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Display(Name = "Job Link")]
        public string JobLink { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;

        [Display(Name = "Notes")]
        public string Notes { get; set; } = string.Empty;

        [Display(Name = "Date Applied")]
        [DataType(DataType.Date)]
        public DateTime DateApplied { get; set; }

        // Computed property - shows a friendly label for how long ago this was applied
        [Display(Name = "Days Since Applied")]
        public int DaysSinceApplied => (DateTime.Now - DateApplied).Days;
    }
}
