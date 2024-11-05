using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;

namespace JobBoard.Core.Services
{
    public class JobListingService : IJobListingService
    {
        private readonly IJobListingRepository _jobListingRepository;

        public JobListingService(IJobListingRepository jobListingRepository) 
        {
            _jobListingRepository = jobListingRepository;
        }

        public async Task<JobListingResponseDto> AddJobListingAsync(JobListingAddRequestDto? jobListingAddRequestDto)
        {
            if(jobListingAddRequestDto == null)
            {
                throw new ArgumentNullException(nameof(jobListingAddRequestDto));
            }

            JobListing jobListing = jobListingAddRequestDto.ToJobListing();
            await _jobListingRepository.AddJobListingAsync(jobListing);
            return jobListing.ToJobListingResponse();
        }

        public async Task<List<JobListingResponseDto>> GetAllJobListingsAsync()
        {
            List<JobListing> jobListings = await _jobListingRepository.GetAllJobListingsAsync();
            return jobListings.Select(j => j.ToJobListingResponse()).ToList();
        }

        public async Task<List<JobListingResponseDto>> GetJobListingsByCityAndState(string? cityAndState)
        {
            List<JobListing> jobListings = await _jobListingRepository.GetJobListingsByCityAndStateAsync(cityAndState);
            return jobListings.Select(j => j.ToJobListingResponse()).ToList();
        }

        public async Task<JobListingResponseDto> UpdateJobListingAsync(JobListingUpdateRequestDto? jobListingUpdateRequest)
        {
            if(jobListingUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(jobListingUpdateRequest));
            }

            JobListing? matchingJobListing = await _jobListingRepository.GetJobListingByJobID(jobListingUpdateRequest.JobID);

            if(matchingJobListing == null)
            {
                throw new ArgumentNullException(nameof(matchingJobListing));
            }

            matchingJobListing.JobTitle = jobListingUpdateRequest.JobTitle;
            matchingJobListing.CompanyName = jobListingUpdateRequest.CompanyName;
            matchingJobListing.Email = jobListingUpdateRequest.Email;
            matchingJobListing.CityAndState = jobListingUpdateRequest.CityAndState;
            matchingJobListing.PayRange = jobListingUpdateRequest.PayRange;
            matchingJobListing.JobType = jobListingUpdateRequest.JobType.ToString();
            matchingJobListing.JobPostedDate = jobListingUpdateRequest.JobPostedDate;
            matchingJobListing.FullDescription = jobListingUpdateRequest.FullDescription;

            await _jobListingRepository.UpdateJobListingAsync(matchingJobListing);
            return matchingJobListing.ToJobListingResponse();
        }

        public async Task<bool> DeleteJobListingAsync(Guid jobID)
        {
            if (jobID == null)
            {
                throw new ArgumentNullException(nameof(jobID));
            }

            JobListing jobListing = await _jobListingRepository.GetJobListingByJobID(jobID);

            if(jobListing == null) { return false; }

            await _jobListingRepository.DeleteJobListingByIDAsync(jobID);
            return true;
        }

    }
}
