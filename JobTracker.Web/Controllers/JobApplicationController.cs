using JobTracker.BusinessLogic.Interfaces;
using JobTracker.DataLogic.Models;
using JobTracker.ViewLogic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Web.Controllers
{
    // [Authorize] means EVERY action in this controller requires the user to be logged in.
    // If they're not logged in, they'll be redirected to the Login page automatically.
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IJobApplicationService _service;

        // UserManager lets us get the currently logged-in user's ID
        private readonly UserManager<ApplicationUser> _userManager;

        public JobApplicationController(IJobApplicationService service,
                                         UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        // -----------------------------------------------------------------------
        // INDEX - List all job applications for the logged-in user
        // -----------------------------------------------------------------------
        // GET: /JobApplication
        public async Task<IActionResult> Index()
        {
            // Get the ID of whoever is currently logged in
            var userId = _userManager.GetUserId(User)!;

            // Fetch only THIS user's applications from the service layer
            var applications = await _service.GetAllApplicationsAsync(userId);

            // Map the database models to ViewModels before sending to the view
            // We never pass raw database models to the view - we always use ViewModels
            var viewModels = applications.Select(a => new JobApplicationViewModel
            {
                Id = a.Id,
                CompanyName = a.CompanyName,
                JobTitle = a.JobTitle,
                JobLink = a.JobLink,
                Status = a.Status,
                Notes = a.Notes,
                DateApplied = a.DateApplied
            }).ToList();

            return View(viewModels);
        }

        // -----------------------------------------------------------------------
        // CREATE - Show and process the "Add new application" form
        // -----------------------------------------------------------------------

        // GET: /JobApplication/Create - Show the empty form
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateJobApplicationViewModel());
        }

        // POST: /JobApplication/Create - Process the submitted form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Validation failed - send the user back to the form with error messages
                return View(model);
            }

            // Map the ViewModel back to a domain model (the object EF Core saves to the database)
            var jobApplication = new JobApplication
            {
                CompanyName = model.CompanyName,
                JobTitle = model.JobTitle,
                JobLink = model.JobLink,
                Status = model.Status,
                Notes = model.Notes,
                DateApplied = model.DateApplied,
                UserId = _userManager.GetUserId(User)! // Attach to the logged-in user
            };

            await _service.AddApplicationAsync(jobApplication);

            // Show a success message on the next page using TempData (survives a redirect)
            TempData["SuccessMessage"] = $"Your application to {model.CompanyName} has been saved!";

            return RedirectToAction(nameof(Index));
        }

        // -----------------------------------------------------------------------
        // EDIT - Show and process the "Edit application" form
        // -----------------------------------------------------------------------

        // GET: /JobApplication/Edit/5 - Show the form pre-filled with existing data
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var application = await _service.GetApplicationByIdAsync(id, userId);

            if (application == null)
            {
                // Application not found, or it belongs to another user
                return NotFound();
            }

            // Map to EditViewModel so the form is pre-filled with current values
            var model = new EditJobApplicationViewModel
            {
                Id = application.Id,
                CompanyName = application.CompanyName,
                JobTitle = application.JobTitle,
                JobLink = application.JobLink,
                Status = application.Status,
                Notes = application.Notes,
                DateApplied = application.DateApplied
            };

            return View(model);
        }

        // POST: /JobApplication/Edit/5 - Save the edited data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditJobApplicationViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(); // The ID in the URL doesn't match the form - something is wrong
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User)!;

            // Map back to the domain model for saving
            var jobApplication = new JobApplication
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                JobTitle = model.JobTitle,
                JobLink = model.JobLink,
                Status = model.Status,
                Notes = model.Notes,
                DateApplied = model.DateApplied,
                UserId = userId // Make sure we preserve the owner
            };

            await _service.UpdateApplicationAsync(jobApplication);

            TempData["SuccessMessage"] = $"Application to {model.CompanyName} has been updated!";

            return RedirectToAction(nameof(Index));
        }

        // -----------------------------------------------------------------------
        // DETAILS - View a single application in full detail
        // -----------------------------------------------------------------------

        // GET: /JobApplication/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var application = await _service.GetApplicationByIdAsync(id, userId);

            if (application == null)
            {
                return NotFound();
            }

            var model = new JobApplicationViewModel
            {
                Id = application.Id,
                CompanyName = application.CompanyName,
                JobTitle = application.JobTitle,
                JobLink = application.JobLink,
                Status = application.Status,
                Notes = application.Notes,
                DateApplied = application.DateApplied
            };

            return View(model);
        }

        // -----------------------------------------------------------------------
        // DELETE - Confirm and delete an application
        // -----------------------------------------------------------------------

        // GET: /JobApplication/Delete/5 - Show a confirmation page
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var application = await _service.GetApplicationByIdAsync(id, userId);

            if (application == null)
            {
                return NotFound();
            }

            var model = new JobApplicationViewModel
            {
                Id = application.Id,
                CompanyName = application.CompanyName,
                JobTitle = application.JobTitle,
                Status = application.Status,
                DateApplied = application.DateApplied
            };

            return View(model);
        }

        // POST: /JobApplication/Delete/5 - Actually delete after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            await _service.DeleteApplicationAsync(id, userId);

            TempData["SuccessMessage"] = "Application has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
