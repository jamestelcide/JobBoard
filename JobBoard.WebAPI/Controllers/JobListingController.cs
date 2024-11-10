﻿using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobBoard.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobListingController : ControllerBase
    {
        private readonly IJobListingService _jobListingService;
        private readonly ILogger<JobListingController> _logger;

        public JobListingController(IJobListingService jobListingService, ILogger<JobListingController> logger)
        {
            _jobListingService = jobListingService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all JobListings.
        /// </summary>
        /// <returns>List of JobListings.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobListingResponseDto>>> GetJobListings()
        {
            _logger.LogInformation("Attempting to retrieve all job listings");

            var allJobListings = await _jobListingService.GetAllJobListingsAsync();

            _logger.LogInformation("Retrieved {Count} job listings", allJobListings.Count);
            return Ok(allJobListings);
        }

        /// <summary>
        /// Retrieves JobListings based on the specified city and state.
        /// </summary>
        /// <param name="cityAndState">The city and state to filter JobListings by.</param>
        /// <returns>List of JobListings matching the city and state.</returns>
        [HttpGet("{cityAndState}")]
        public async Task<ActionResult<IEnumerable<JobListingResponseDto>>> GetJobListingsByCityAndState(string cityAndState)
        {
            _logger.LogInformation("Attempting to retrieve job listings for CityAndState: {CityAndState}", cityAndState);

            var jobListings = await _jobListingService.GetJobListingsByCityAndState(cityAndState);

            if (jobListings.Count == 0)
            {
                _logger.LogWarning("No job listings found for CityAndState: {CityAndState}", cityAndState);
                return NotFound("No match found for CityAndState.");
            }

            _logger.LogInformation("Retrieved {Count} job listings for CityAndState: {CityAndState}", jobListings.Count, cityAndState);
            return Ok(jobListings);
        }

        /// <summary>
        /// Updates an existing JobListing.
        /// </summary>
        /// <param name="jobID">The JobID of the JobListing to update.</param>
        /// <param name="jobListingUpdateRequest">The updated JobListing details.</param>
        /// <returns>Status of the update operation.</returns>
        [HttpPut("{jobID}")]
        public async Task<IActionResult> PutJobListing(Guid jobID, JobListingUpdateRequestDto jobListingUpdateRequest)
        {
            _logger.LogInformation("Attempting to update job listing with JobID: {JobID}", jobID);

            if (jobID != jobListingUpdateRequest.JobID)
            {
                _logger.LogWarning("JobID in URL ({UrlJobID}) does not match JobID in body ({BodyJobID})", jobID, jobListingUpdateRequest.JobID);
                return BadRequest("JobID in the URL does not match JobID in the body.");
            }

            try
            {
                var updatedJobListing = await _jobListingService.UpdateJobListingAsync(jobListingUpdateRequest);
                _logger.LogInformation("Successfully updated job listing with JobID: {JobID}", jobID);
                return Ok(updatedJobListing);
            }
            catch (ArgumentNullException)
            {
                _logger.LogWarning("Job listing with JobID: {JobID} not found for update", jobID);
                return NotFound("JobListing not found.");
            }
        }

        /// <summary>
        /// Adds a new JobListing.
        /// </summary>
        /// <param name="jobListingAddRequest">The JobListing details to add.</param>
        /// <returns>The created JobListing.</returns>
        [HttpPost]
        public async Task<ActionResult<JobListingResponseDto>> PostJobListing(JobListingAddRequestDto jobListingAddRequest)
        {
            _logger.LogInformation("Attempting to add a new job listing");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for new job listing");
                return BadRequest(ModelState);
            }

            var createdJobListing = await _jobListingService.AddJobListingAsync(jobListingAddRequest);

            if (createdJobListing == null)
            {
                _logger.LogError("Failed to create job listing");
                return NotFound("Failed to create job listing.");
            }

            _logger.LogInformation("Successfully created job listing with JobID: {JobID}", createdJobListing.JobID);
            return CreatedAtAction(nameof(GetJobListingsByCityAndState), new { cityAndState = createdJobListing.CityAndState }, createdJobListing);
        }

        /// <summary>
        /// Deletes a JobListing by JobID.
        /// </summary>
        /// <param name="jobID">The JobID of the JobListing to delete.</param>
        /// <returns>Status of the delete operation.</returns>
        [HttpDelete("{jobID}")]
        public async Task<IActionResult> DeleteJobListing(Guid jobID)
        {
            _logger.LogInformation("Attempting to delete job listing with JobID: {JobID}", jobID);

            var isDeleted = await _jobListingService.DeleteJobListingAsync(jobID);

            if (isDeleted)
            {
                _logger.LogInformation("Successfully deleted job listing with JobID: {JobID}", jobID);
                return NoContent();
            }
            else
            {
                _logger.LogWarning("Job listing with JobID: {JobID} not found for deletion", jobID);
                return NotFound("JobListing not found.");
            }
        }

        /// <summary>
        /// Checks if a JobListing with the specified JobID exists.
        /// </summary>
        /// <param name="jobID">The JobID of the JobListing to check.</param>
        /// <returns>True if JobListing exists, otherwise false.</returns>
        private async Task<bool> JobListingExists(Guid jobID)
        {
            _logger.LogInformation("Checking if job listing exists with JobID: {JobID}", jobID);

            var jobListings = await _jobListingService.GetAllJobListingsAsync();
            bool exists = jobListings.Any(j => j.JobID == jobID);

            _logger.LogInformation("Job listing with JobID: {JobID} exists: {Exists}", jobID, exists);
            return exists;
        }
    }
}