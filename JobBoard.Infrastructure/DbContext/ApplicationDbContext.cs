using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public ApplicationDbContext()
        {

        }

        public virtual DbSet<JobListing> JobListings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<JobListing>().ToTable("JobListings");

            string jobListingsJson = System.IO.File.ReadAllText("Seed/JobListingsSeed.json");

            List<JobListing>? jobListings = System.Text.Json.JsonSerializer
                .Deserialize<List<JobListing>>(jobListingsJson);

            if(jobListings != null) 
            {
                foreach(JobListing jobListing in jobListings)
                {
                    builder.Entity<JobListing>().HasData(jobListing);
                }
            }
        }
    }
}
