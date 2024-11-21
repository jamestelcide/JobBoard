using JobBoard.Core.Domain.Entities;

namespace JobBoard.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing JobListing entity
    /// </summary>
    public interface IJobListingRepository
    {
        /// <summary>
        /// Adds a JobListing object to the data store
        /// </summary>
        /// <param name="jobListing">JobListing object to add</param>
        /// <returns>Returns the JobListing object after adding it to the table</returns>
        Task<JobListing> AddJobListingAsync(JobListing jobListing);

        /// <summary>
        /// Returns all JobListings in the data store
        /// </summary>
        /// <returns>List of JobListing objects from the table</returns>
        Task<List<JobListing>> GetAllJobListingsAsync();

        /// <summary>
        /// Returns a list of JobListing objects based on the given CityAndState
        /// </summary>
        /// <param name="cityAndState">CityAndState to search</param>
        /// <returns>A list of JobListing objects or null</returns>
        Task<List<JobListing>> GetJobListingsByCityAndStateAsync(string cityAndState);

        /// <summary>
        /// Returns a JobListing object based on the given JobID
        /// </summary>
        /// <param name="jobID">JobID to search</param>
        /// <returns>A JobListing object or null</returns>
        Task<JobListing?> GetJobListingByJobID(Guid jobID);

        /// <summary>
        /// Updates a JobListing object based on the given JobID
        /// </summary>
        /// <param name="jobListing">JobListing object to update</param>
        /// <returns>Returns the updated JobListing object</returns>
        Task<JobListing> UpdateJobListingAsync(JobListing jobListing);

        /// <summary>
        /// Deletes a JobListing object based on the JobID
        /// </summary>
        /// <param name="jobID">JobID to search</param>
        /// <returns>Returns true, if delete is successful, otherwise false</returns>
        Task<bool> DeleteJobListingByIDAsync(Guid jobID);

        /// <summary>
        /// Checks whether a JobListing exists in the database for the given JobID.
        /// </summary>
        /// <param name="jobID">The unique identifier of the JobListing to check.</param>
        /// <returns>Task result containing a boolean indicating whether the JobListing exists (true) or not (false).</returns>
        Task<bool> DoesJobListingExistAsync(Guid jobID);
    }
}
