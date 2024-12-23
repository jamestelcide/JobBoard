﻿using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace JobBoard.Core.Services
{
    public class JobListingService : IJobListingService
    {
        private readonly IJobListingRepository _jobListingRepository;
        private readonly ILogger<JobListingService> _logger;

        public JobListingService(IJobListingRepository jobListingRepository, ILogger<JobListingService> logger)
        {
            _jobListingRepository = jobListingRepository;
            _logger = logger;
        }

        public async Task<JobListingResponseDto> AddJobListingAsync(JobListingAddRequestDto? jobListingAddRequestDto)
        {
            _logger.LogInformation("Adding a new JobListing");

            if (jobListingAddRequestDto == null)
            {
                _logger.LogWarning("AddJobListingAsync called with null jobListingAddRequestDto");
                throw new ArgumentNullException(nameof(jobListingAddRequestDto));
            }

            JobListing jobListing = jobListingAddRequestDto.ToJobListing();
            jobListing.JobID = Guid.NewGuid();

            await _jobListingRepository.AddJobListingAsync(jobListing);

            _logger.LogInformation("JobListing added successfully with JobID: {JobID}", jobListing.JobID);
            return jobListing.ToJobListingResponse();
        }

        public async Task<List<JobListingResponseDto>> GetAllJobListingsAsync()
        {
            _logger.LogInformation("Retrieving all JobListing");

            List<JobListing> jobListings = await _jobListingRepository.GetAllJobListingsAsync();
            _logger.LogInformation("Retrieved {Count} JobListing", jobListings.Count);

            return jobListings.Select(j => j.ToJobListingResponse()).ToList();
        }

        public async Task<JobListingResponseDto?> GetJobListingByIDAsync(Guid? jobID)
        {
            _logger.LogInformation("Retrieving JobListing by ID");

            if (jobID == null) { return null; }

            JobListing? jobListing = await _jobListingRepository.GetJobListingByJobID(jobID.Value);

            if (jobListing == null) { return null; }

            _logger.LogInformation("Retrieved JobListing");

            return jobListing.ToJobListingResponse();
        }

        public async Task<List<JobListingResponseDto>> GetJobListingsByCityAndState(string? cityAndState)
        {
            _logger.LogInformation("Retrieving JobListings for city and state: {CityAndState}", cityAndState);
            
            if (cityAndState == null)
            {
                throw new ArgumentNullException(nameof(cityAndState));
            }

            List<JobListing> jobListings = await _jobListingRepository.GetJobListingsByCityAndStateAsync(cityAndState);
            _logger.LogInformation("Retrieved {Count} JobListings for city and state: {CityAndState}", jobListings.Count, cityAndState);

            return jobListings.Select(j => j.ToJobListingResponse()).ToList();
        }

        public async Task<JobListingResponseDto> UpdateJobListingAsync(JobListingUpdateRequestDto? jobListingUpdateRequest)
        {
            _logger.LogInformation("Updating JobListing with JobID: {JobID}", jobListingUpdateRequest?.JobID);

            if (jobListingUpdateRequest == null)
            {
                _logger.LogWarning("UpdateJobListingAsync called with null jobListingUpdateRequest");
                throw new ArgumentNullException(nameof(jobListingUpdateRequest), "JobListing update request cannot be null.");
            }

            JobListing? matchingJobListing = await _jobListingRepository.GetJobListingByJobID(jobListingUpdateRequest.JobID);

            if (matchingJobListing == null)
            {
                _logger.LogWarning("JobListing with JobID: {JobID} not found", jobListingUpdateRequest.JobID);
                throw new ArgumentException($"JobListing with ID {jobListingUpdateRequest.JobID} not found.");
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

            _logger.LogInformation("JobListing with JobID: {JobID} updated successfully", jobListingUpdateRequest.JobID);
            return matchingJobListing.ToJobListingResponse();
        }

        public async Task<bool> DeleteJobListingAsync(Guid jobID)
        {
            _logger.LogInformation("Deleting JobListing with JobID: {JobID}", jobID);

            var jobListing = await _jobListingRepository.GetJobListingByJobID(jobID);
            if (jobListing == null)
            {
                _logger.LogWarning("JobListing with JobID: {JobID} not found", jobID);
                return false;
            }

            await _jobListingRepository.DeleteJobListingByIDAsync(jobID);
            _logger.LogInformation("JobListing with JobID: {JobID} deleted successfully", jobID);

            return true;
        }
    }
}
