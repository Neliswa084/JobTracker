using JobTracker.DataLogic.Models;

namespace JobTracker.Repository.Interfaces
{
    // This interface defines WHAT the repository can do, but not HOW it does it.
    // This is a key concept in Layered Architecture - the Business Logic layer only
    // knows about this interface, not the actual implementation.
    // This makes the code easy to test and swap out later (e.g. switch from SQL to something else).
    public interface IJobApplicationRepository
    {
        // Get all job applications for a specific user
        Task<IEnumerable<JobApplication>> GetAllByUserIdAsync(string userId);

        // Get a single job application by its ID (and verify it belongs to the user)
        Task<JobApplication?> GetByIdAsync(int id, string userId);

        // Save a new job application to the database
        Task AddAsync(JobApplication jobApplication);

        // Update an existing job application in the database
        Task UpdateAsync(JobApplication jobApplication);

        // Delete a job application from the database
        Task DeleteAsync(int id, string userId);
    }
}
