using JobTracker.DataLogic.Models;

namespace JobTracker.BusinessLogic.Interfaces
{
    // The Service interface defines the "business operations" of the application.
    // The Controller (in the Web layer) will only talk to this interface,
    // never directly to the Repository. This keeps each layer focused on its job.
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplication>> GetAllApplicationsAsync(string userId);

        Task<JobApplication?> GetApplicationByIdAsync(int id, string userId);

        Task AddApplicationAsync(JobApplication jobApplication);

        Task UpdateApplicationAsync(JobApplication jobApplication);

        Task DeleteApplicationAsync(int id, string userId);
    }
}
