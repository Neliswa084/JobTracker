using JobTracker.DataLogic.Data;
using JobTracker.DataLogic.Models;
using JobTracker.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Repository.Implementations
{
    // This class implements IJobApplicationRepository.
    // It is the ONLY place in the entire project that directly talks to the database.
    // All other layers go through this class to read or write data.
    public class JobApplicationRepository : IJobApplicationRepository
    {
        // We inject AppDbContext through the constructor (Dependency Injection).
        // ASP.NET Core automatically provides the DbContext when this class is created.
        private readonly AppDbContext _context;

        public JobApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all applications that belong to a specific user.
        // We filter by UserId so users NEVER see each other's applications.
        // OrderByDescending shows the most recently applied jobs first.
        public async Task<IEnumerable<JobApplication>> GetAllByUserIdAsync(string userId)
        {
            return await _context.JobApplications
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.DateApplied)
                .ToListAsync();
        }

        // Get one application by ID, but also check the UserId as a security measure.
        // Returns null if not found or if the application belongs to a different user.
        public async Task<JobApplication?> GetByIdAsync(int id, string userId)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);
        }

        // Add a new job application to the database.
        // async/await is used so the app doesn't freeze while waiting for the database.
        public async Task AddAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync(); // This actually executes the INSERT SQL
        }

        // Update an existing job application.
        // EF Core tracks changes, so we just mark the entity as modified and save.
        public async Task UpdateAsync(JobApplication jobApplication)
        {
            _context.JobApplications.Update(jobApplication);
            await _context.SaveChangesAsync(); // Executes the UPDATE SQL
        }

        // Delete a job application. We check UserId again for security.
        public async Task DeleteAsync(int id, string userId)
        {
            var jobApplication = await GetByIdAsync(id, userId);

            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
                await _context.SaveChangesAsync(); // Executes the DELETE SQL
            }
        }
    }
}
