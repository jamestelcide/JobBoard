﻿using JobBoard.Core.Dto;

namespace JobBoard.Core.ServiceContracts
{
    /// <summary>
    /// Represents buisness logic for managing JobListing entity
    /// </summary>
    public interface IJobListingService
    {
        /// <summary>
        /// Adds a JobListing object to the list
        /// </summary>
        /// <param name="jobListingAddRequestDto">JobListing to add</param>
        /// <returns>Returns the JobListing object with a newly generated JobID</returns>
        Task<JobListingResponseDto> AddJobListingAsync(JobListingAddRequestDto? jobListingAddRequestDto);

        /// <summary>
        /// Returns all JobListings from the list
        /// </summary>
        /// <returns>All JobListings from the list as a list of JobListingResponseDto</returns>
        Task<List<JobListingResponseDto>> GetAllJobListingsAsync();

        /// <summary>
        /// Returns all JobListings with the same CityAndState Value
        /// </summary>
        /// <param name="cityAndState">CityAndState to search</param>
        /// <returns>A list all JobListings with the same CityAndState Value</returns>
        Task<List<JobListingResponseDto>> GetJobListingsByCityAndState(string? cityAndState);

        /// <summary>
        /// Updates the specified JobListing details based on the given JobID
        /// </summary>
        /// <param name="JobListingUpdateRequest">JobListing details to update, including JobID</param>
        /// <returns>Returns the JobListingResponse object after updating</returns>
        Task<JobListingResponseDto> UpdateJobListingAsync(JobListingUpdateRequestDto? jobListingUpdateRequest);

        /// <summary>
        /// Deletes a JobListing based on the given JobID
        /// </summary>
        /// <param name="JobID"></param>
        /// <returns>Returns true if delete was successful; otherwise returns false</returns>
        Task<bool> DeleteJobListingAsync(Guid jobID);
    }
}
