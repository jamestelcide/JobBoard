using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Repositories
{
    public class JobListingRepository : IJobListingRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<JobListingRepository> _logger;

        public JobListingRepository(ApplicationDbContext db, ILogger<JobListingRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<JobListing> AddJobListingAsync(JobListing jobListing)
        {
            _logger.LogInformation("Adding new job listing with JobID: {JobID}", jobListing.JobID);

            _db.JobListings.Add(jobListing);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Job listing with JobID: {JobID} added successfully", jobListing.JobID);
            return jobListing;
        }

        public async Task<List<JobListing>> GetAllJobListingsAsync()
        {
            _logger.LogInformation("Retrieving all job listings");

            List<JobListing> jobListings = await _db.JobListings.ToListAsync();

            _logger.LogInformation("Retrieved {Count} job listings", jobListings.Count);
            return jobListings;
        }

        public async Task<List<JobListing>> GetJobListingsByCityAndStateAsync(string cityAndState)
        {
            _logger.LogInformation("Retrieving job listings for city and state: {CityAndState}", cityAndState);

            List<JobListing> jobListings = await _db.JobListings
                .Where(j => j.CityAndState == cityAndState)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} job listings for city and state: {CityAndState}", jobListings.Count, cityAndState);
            return jobListings;
        }

        public async Task<JobListing?> GetJobListingByJobID(Guid jobID)
        {
            _logger.LogInformation("Retrieving job listing with JobID: {JobID}", jobID);

            JobListing? jobListing = await _db.JobListings.FirstOrDefaultAsync(j => j.JobID == jobID);

            if (jobListing == null)
            {
                _logger.LogWarning("No job listing found with JobID: {JobID}", jobID);
            }
            else
            {
                _logger.LogInformation("Job listing with JobID: {JobID} retrieved successfully", jobID);
            }

            return jobListing;
        }

        public async Task<JobListing> UpdateJobListingAsync(JobListing jobListing)
        {
            _logger.LogInformation("Updating job listing with JobID: {JobID}", jobListing.JobID);

            JobListing? matchingJobListing = await _db.JobListings
                .FirstOrDefaultAsync(j => j.JobID == jobListing.JobID);

            if (matchingJobListing == null)
            {
                _logger.LogWarning("No job listing found with JobID: {JobID} for update", jobListing.JobID);
                return jobListing;
            }

            matchingJobListing.JobTitle = jobListing.JobTitle;
            matchingJobListing.CompanyName = jobListing.CompanyName;
            matchingJobListing.CityAndState = jobListing.CityAndState;
            matchingJobListing.PayRange = jobListing.PayRange;
            matchingJobListing.JobType = jobListing.JobType;
            matchingJobListing.FullDescription = jobListing.FullDescription;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Job listing with JobID: {JobID} updated successfully", jobListing.JobID);
            return matchingJobListing;
        }

        public async Task<bool> DeleteJobListingByIDAsync(Guid jobID)
        {
            _logger.LogInformation("Deleting job listing with JobID: {JobID}", jobID);

            var jobListing = await _db.JobListings.FirstOrDefaultAsync(j => j.JobID == jobID);

            if (jobListing != null)
            {
                _db.JobListings.Remove(jobListing);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Job listing with JobID: {JobID} deleted successfully", jobID);
                return true;
            }

            _logger.LogWarning("No job listing found with JobID: {JobID} to delete", jobID);
            return false;
        }

        public async Task<bool> DoesJobListingExistAsync(Guid jobID)
        {
            return await (_db.JobListings?.AnyAsync(j => j.JobID == jobID) ?? Task.FromResult(false));
        }
    }
}
