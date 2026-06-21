using JobTracker.DataLogic.Models;
using JobTracker.ViewLogic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Web.Controllers
{
    // This controller handles everything related to user accounts:
    // - Registering a new account
    // - Logging in
    // - Logging out
    public class AccountController : Controller
    {
        // UserManager handles creating users, hashing passwords, etc.
        private readonly UserManager<ApplicationUser> _userManager;

        // SignInManager handles logging users in and out (creating/destroying the auth cookie)
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // -----------------------------------------------------------------------
        // REGISTER
        // -----------------------------------------------------------------------

        // GET: /Account/Register - show the registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register - process the registration form
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks (cross-site request forgery)
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Check that all [Required] fields are filled in correctly
            if (!ModelState.IsValid)
            {
                return View(model); // Return the form with validation error messages
            }

            // Create a new ApplicationUser object from the form data
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email, // Identity uses UserName as the login identifier
                Email = model.Email
            };

            // Try to create the user in the database (Identity hashes the password automatically)
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User created successfully - log them in right away
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "JobApplication"); // Go to their dashboard
            }

            // If creation failed (e.g. email already taken), show the errors on the form
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // -----------------------------------------------------------------------
        // LOGIN
        // -----------------------------------------------------------------------

        // GET: /Account/Login - show the login form
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // returnUrl is the page the user was trying to access before being asked to log in
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login - process the login form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Try to sign in with the email and password provided
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,  // If true, the login cookie persists after browser is closed
                lockoutOnFailure: false); // Don't lock the account after failed attempts (for simplicity)

            if (result.Succeeded)
            {
                // If there's a returnUrl, send them back to where they were going
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "JobApplication");
            }

            // Login failed - show a generic error (don't say "email not found" as that's a security risk)
            ModelState.AddModelError(string.Empty, "Invalid email or password. Please try again.");
            return View(model);
        }

        // -----------------------------------------------------------------------
        // LOGOUT
        // -----------------------------------------------------------------------

        // POST: /Account/Logout - sign the user out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Destroys the authentication cookie
            return RedirectToAction("Index", "Home");
        }
    }
}
