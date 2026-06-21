using System.ComponentModel.DataAnnotations;

namespace JobTracker.ViewLogic.ViewModels
{
    // ViewModel for the Login page - collects email and password to authenticate the user
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        // "Remember me" checkbox - keeps the user logged in across browser sessions
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
