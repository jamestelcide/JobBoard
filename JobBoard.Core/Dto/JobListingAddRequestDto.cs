using JobBoard.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Core.Dto
{
    /// <summary>
    /// Dto for inserting a new JobListing
    /// </summary>
    public class JobListingAddRequestDto
    {
        [Required(ErrorMessage = "Job title can not be blank!")]
        public string JobTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company Name can not be blank!")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email can NOT be blank")]
        [EmailAddress(ErrorMessage = "Please use valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "City and state can not be blank!")]
        public string CityAndState { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pay range can not be blank!")]
        public string PayRange { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job type can not be blank!")]
        public string JobType { get; set; } = string.Empty;

        [Required(ErrorMessage = "JobPostedDate can not be blank!")]
        public DateTime JobPostedDate { get; set; }

        [Required(ErrorMessage = "Description can not be blank!")]
        public string FullDescription { get; set; } = string.Empty;

        /// <summary>
        /// Converts the current object of JobListingAddRequest into a new object of JobListing type
        /// </summary>
        /// <returns></returns>
        public JobListing ToJobListing()
        {
            return new JobListing()
            {
                JobTitle = JobTitle,
                CompanyName = CompanyName,
                Email = Email,
                CityAndState = CityAndState,
                PayRange = PayRange,
                JobType = JobType,
                JobPostedDate = JobPostedDate,
                FullDescription = FullDescription
            };
        }
    }
}
