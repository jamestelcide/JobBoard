using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobListingController : ControllerBase
    {
        private readonly IJobListingService _jobListingService;

        public JobListingController(IJobListingService jobListingService)
        {
            _jobListingService = jobListingService;
        }

        /// <summary>
        /// Retrieves all JobListings.
        /// </summary>
        /// <returns>List of JobListings.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobListingResponseDto>>> GetJobListings()
        {
            var allJobListings = await _jobListingService.GetAllJobListingsAsync();
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
            var jobListings = await _jobListingService.GetJobListingsByCityAndState(cityAndState);
            return jobListings.Count == 0 ? NotFound("No match found for CityAndState.") : Ok(jobListings);
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
            if (jobID != jobListingUpdateRequest.JobID)
            {
                return BadRequest("JobID in the URL does not match JobID in the body.");
            }

            try
            {
                var updatedJobListing = await _jobListingService.UpdateJobListingAsync(jobListingUpdateRequest);
                return Ok(updatedJobListing);
            }
            catch (ArgumentNullException)
            {
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdJobListing = await _jobListingService.AddJobListingAsync(jobListingAddRequest);
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
            var isDeleted = await _jobListingService.DeleteJobListingAsync(jobID);
            return isDeleted ? NoContent() : NotFound("JobListing not found.");
        }

        /// <summary>
        /// Checks if a JobListing with the specified JobID exists.
        /// </summary>
        /// <param name="jobID">The JobID of the JobListing to check.</param>
        /// <returns>True if JobListing exists, otherwise false.</returns>
        private async Task<bool> JobListingExists(Guid jobID)
        {
            var jobListings = await _jobListingService.GetAllJobListingsAsync();
            return jobListings.Any(j => j.JobID == jobID);
        }
    }
}
