using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Core.Dto
{
    /// <summary>
    /// Represents the Dto class that contains the JobListing details to update
    /// </summary>
    public class JobListingUpdateRequestDto
    {
        [Required(ErrorMessage = "JobID can't be blank")]
        public Guid JobID { get; set; }
        
        [Required(ErrorMessage = "Job title can't be blank")]
        public string JobTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name can't be blank")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email can NOT be blank")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "City and state can't be blank")]
        public string CityAndState { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pay range can't be blank")]
        public string PayRange { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job type can't be blank")]
        public JobTypeOptions JobType { get; set; }
        
        [Required(ErrorMessage = "JobPostedDate can not be blank!")]
        public DateTime JobPostedDate { get; set; }

        [Required(ErrorMessage = "Full description can't be blank")]
        public string FullDescription { get; set; } = string.Empty;

        public JobListing ToJobListing()
        {
            return new JobListing
            {
                JobID = JobID,
                JobTitle = JobTitle,
                CompanyName = CompanyName,
                Email = Email,
                CityAndState = CityAndState,
                PayRange = PayRange,
                JobType = JobType.ToString(),
                JobPostedDate = JobPostedDate,
                FullDescription = FullDescription
            };
        }
    }
}
