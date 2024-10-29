using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("AddJobListingAsync of JobListingRepository");

            _db.JobListings.Add(jobListing);
            await _db.SaveChangesAsync();
            return jobListing;
        }

        public async Task<List<JobListing>> GetAllJobListingsAsync()
        {
            _logger.LogInformation("GetAllJobListingsAsync of JobListingRepository");

            return await _db.JobListings.ToListAsync();
        }

        public async Task<JobListing?> GetJobListingByCityAndStateAsync(string cityAndState)
        {
            _logger.LogInformation("GetJobListingByCityAndStateAsync of JobListingRepository");

            return await _db.JobListings.FirstOrDefaultAsync(j => j.CityAndState == cityAndState);
        }

        public async Task<JobListing> UpdateJobListingAsync(JobListing jobListing)
        {
            _logger.LogInformation("UpdateJobListingAsync of JobListingRepository");

            JobListing? matchingJobListing = await _db.JobListings
                .FirstOrDefaultAsync(j => j.JobID == jobListing.JobID);

            if (matchingJobListing == null) { return jobListing; }

            matchingJobListing.JobTitle = jobListing.JobTitle;
            matchingJobListing.CompanyName = jobListing.CompanyName;
            matchingJobListing.CityAndState = jobListing.CityAndState;
            matchingJobListing.PayRange = jobListing.PayRange;
            matchingJobListing.JobType = jobListing.JobType;
            matchingJobListing.FullDescription = jobListing.FullDescription;

            await _db.SaveChangesAsync();
            return matchingJobListing;
        }

        public async Task<bool> DeleteJobListingByIDAsync(Guid jobID)
        {
            _logger.LogInformation("DeleteJobListingByIDAsync of JobListingRepository");

            _db.JobListings.RemoveRange(_db.JobListings.Where(j => j.JobID == jobID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
