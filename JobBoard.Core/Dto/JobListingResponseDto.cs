using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Enums;

namespace JobBoard.Core.Dto
{
    /// <summary>
    /// Dto class that is used as a return type for most methods of JobListingService
    /// </summary>
    public class JobListingResponseDto
    {
        public Guid JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string CityAndState { get; set; } = string.Empty;
        public string PayRange { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public DateTime JobPostedDate { get; set; }
        public string FullDescription { get; set; } = string.Empty;

        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The JobListingResponse Object to compare</param>
        /// <returns>True or False, whether all JobListing details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if(obj == null) {  return false; }
            if(obj.GetType() != typeof(JobListingResponseDto)) { return false; }

            JobListingResponseDto jobListing = (JobListingResponseDto)obj;

            return JobID == jobListing.JobID &&
            JobTitle == jobListing.JobTitle &&
            CompanyName == jobListing.CompanyName &&
            Email == jobListing.Email &&
            CityAndState == jobListing.CityAndState &&
            PayRange == jobListing.PayRange &&
            JobType == jobListing.JobType &&
            JobPostedDate == jobListing.JobPostedDate &&
            FullDescription == jobListing.FullDescription;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(JobID, JobTitle, CompanyName);
        }

        public JobListingUpdateRequestDto ToJobListingUpdateRequest()
        {
            return new JobListingUpdateRequestDto()
            {
                JobID = JobID,
                JobTitle = JobTitle,
                CompanyName = CompanyName,
                Email = Email,
                CityAndState = CityAndState,
                PayRange = PayRange,
                JobType = (JobTypeOptions)Enum.Parse(typeof(JobTypeOptions), JobType, true),
                JobPostedDate = JobPostedDate,
                FullDescription = FullDescription
            };
        }
    }

    public static class JobListingExtensions
    {
        /// <summary>
        /// An extension method to convert an object of JobListing class into JobListingResponseDto class
        /// </summary>
        /// <param name="jobListing">The JobListing object to convert</param>
        /// <returns>Returns the converted JobListingResponse object</returns>
        public static JobListingResponseDto ToJobListingResponse(this JobListing jobListing)
        {
            return new JobListingResponseDto()
            {
                JobID = jobListing.JobID,
                JobTitle = jobListing.JobTitle,
                CompanyName = jobListing.CompanyName,
                Email = jobListing.Email,
                CityAndState = jobListing.CityAndState,
                PayRange = jobListing.PayRange,
                JobType = jobListing.JobType,
                JobPostedDate = jobListing.JobPostedDate,
                FullDescription = jobListing.FullDescription
            };
        }
    }
}
