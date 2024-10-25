using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Core.Domain.Entities
{
    public class JobListing
    {
        [Key]
        public Guid JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompnayName { get; set; } = string.Empty;
        public string CityAndState { get; set; } = string.Empty;
        public string PayRange { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;

    }
}
