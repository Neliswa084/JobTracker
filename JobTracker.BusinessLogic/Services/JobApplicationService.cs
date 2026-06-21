using JobTracker.BusinessLogic.Interfaces;
using JobTracker.DataLogic.Models;
using JobTracker.Repository.Interfaces;

namespace JobTracker.BusinessLogic.Services
{
    // The Service layer sits between the Controller (Web) and the Repository (Data).
    // Its job is to contain business logic - rules about HOW the application behaves.
    // For example: "A user can only see their own applications" is a business rule.
    // Right now it mostly passes calls through to the repository, but as the project
    // grows, this is where you'd add things like validation, notifications, etc.
    public class JobApplicationService : IJobApplicationService
    {
        // We depend on the interface, not the concrete class - this is called
        // "programming to an interface" and it makes the code more flexible and testable.
        private readonly IJobApplicationRepository _repository;

        public JobApplicationService(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobApplication>> GetAllApplicationsAsync(string userId)
        {
            // Business rule: only return applications for THIS user
            return await _repository.GetAllByUserIdAsync(userId);
        }

        public async Task<JobApplication?> GetApplicationByIdAsync(int id, string userId)
        {
            // Business rule: a user can only view their own application
            return await _repository.GetByIdAsync(id, userId);
        }

        public async Task AddApplicationAsync(JobApplication jobApplication)
        {
            // Business rule: always set the date applied to now if not provided
            if (jobApplication.DateApplied == DateTime.MinValue)
            {
                jobApplication.DateApplied = DateTime.Now;
            }

            await _repository.AddAsync(jobApplication);
        }

        public async Task UpdateApplicationAsync(JobApplication jobApplication)
        {
            await _repository.UpdateAsync(jobApplication);
        }

        public async Task DeleteApplicationAsync(int id, string userId)
        {
            // Business rule: a user can only delete their own application
            await _repository.DeleteAsync(id, userId);
        }
    }
}
