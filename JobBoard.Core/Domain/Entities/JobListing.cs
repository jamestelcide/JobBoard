using System.ComponentModel.DataAnnotations;

namespace JobBoard.Core.Domain.Entities
{
    public class JobListing
    {
        [Key]
        public Guid JobID { get; set; }
        [StringLength(100)]
        [Required]
        public string JobTitle { get; set; } = string.Empty;
        [StringLength(100)]
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        [StringLength(15)]
        [Required]
        public string CityAndState { get; set; } = string.Empty;
        [StringLength(40)]
        [Required]
        public string PayRange { get; set; } = string.Empty;
        [Required]
        public string JobType { get; set; } = string.Empty;
        [Required]
        public string FullDescription { get; set; } = string.Empty;
    }
}
