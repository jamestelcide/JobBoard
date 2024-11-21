using FluentAssertions;
using JobBoard.Core.Dto;
using JobBoard.Core.Enums;
using System.Net;
using System.Net.Http.Json;

namespace JobBoard.IntegrationTests
{
    public class JobListingControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public JobListingControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region GetJobListings
        [Fact]
        public async Task GetJobListings_ShouldReturnAllJobListings()
        {
            // Arrange
            var newJobListing = new JobListingAddRequestDto
            {
                JobTitle = "Software Engineer",
                CompanyName = "Innovative Solutions",
                Email = "careers@innovativesolutions.com",
                CityAndState = "Austin, TX",
                PayRange = "$85,000 - $125,000",
                JobType = JobTypeOptions.FullTime,
                JobPostedDate = DateTime.UtcNow,
                FullDescription = "Looking for a software engineer experienced in C# and .NET."
            };

            var postResponse = await _httpClient.PostAsJsonAsync("/api/JobListing", newJobListing);
            postResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _httpClient.GetAsync("/api/JobListing");

            // Assert
            response.Should().BeSuccessful();
            var jobListings = await response.Content.ReadFromJsonAsync<List<JobListingResponseDto>>();

            jobListings.Should().NotBeNull();
            jobListings.Should().NotBeEmpty();
            jobListings.Should().Contain(j => j.JobTitle == newJobListing.JobTitle &&
            j.CompanyName == newJobListing.CompanyName);
        }

        #endregion

        #region GetJobListingsByCityAndState
        [Fact]
        public async Task GetJobListingsByCityAndState_ShouldReturnNotFound_WhenCityAndStateDoesNotMatch()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/JobListing/citystate/NonExistentCity");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorMessage = await response.Content.ReadAsStringAsync();
            errorMessage.Should().Contain("No match found for CityAndState.");
        }
        #endregion

        #region PostJobListing
        [Fact]
        public async Task PostJobListing_ShouldCreateJobListing_WhenModelIsValid()
        {
            // Arrange
            var jobListingToAdd = new JobListingAddRequestDto
            {
                JobTitle = "Software Engineer",
                CompanyName = "Innovative Solutions",
                Email = "careers@innovativesolutions.com",
                CityAndState = "Atlanta, GA",
                PayRange = "$85,000 - $125,000",
                JobType = JobTypeOptions.FullTime,
                JobPostedDate = DateTime.UtcNow,
                FullDescription = "Looking for a software engineer experienced in C# and .NET."
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/JobListing", jobListingToAdd);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdJobListing = await response.Content.ReadFromJsonAsync<JobListingResponseDto>();
            createdJobListing.Should().NotBeNull();
            createdJobListing.CityAndState.Should().Be(jobListingToAdd.CityAndState);
        }

        [Fact]
        public async Task PostJobListing_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidJobListing = new JobListingAddRequestDto(); // Missing required fields

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/JobListing", invalidJobListing);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region PutJobListing
        [Fact]
        public async Task PutJobListing_ShouldUpdateJobListing_WhenJobIDIsValid()
        {
            // Arrange
            var newJobListing = new JobListingAddRequestDto
            {
                JobTitle = "Junior Developer",
                CompanyName = "Tech Innovators",
                Email = "hr@techinnovators.com",
                CityAndState = "San Francisco, CA",
                PayRange = "$70,000 - $90,000",
                JobType = JobTypeOptions.FullTime,
                JobPostedDate = DateTime.UtcNow,
                FullDescription = "An entry-level position for enthusiastic developers."
            };

            var postResponse = await _httpClient.PostAsJsonAsync("/api/JobListing", newJobListing);
            postResponse.EnsureSuccessStatusCode();

            var createdJobListing = await postResponse.Content.ReadFromJsonAsync<JobListingResponseDto>();
            
            Assert.NotNull(createdJobListing);

            var jobID = createdJobListing.JobID;

            var updateRequest = new JobListingUpdateRequestDto
            {
                JobID = jobID,
                JobTitle = "Software Engineer",
                CompanyName = "Innovative Solutions",
                Email = "careers@innovativesolutions.com",
                CityAndState = "Austin, TX",
                PayRange = "$85,000 - $125,000",
                JobType = JobTypeOptions.FullTime,
                JobPostedDate = DateTime.UtcNow,
                FullDescription = "Looking for a software engineer experienced in C# and .NET."
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"/api/JobListing/{jobID}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedJobListing = await response.Content.ReadFromJsonAsync<JobListingResponseDto>();
            updatedJobListing.Should().NotBeNull();
            updatedJobListing.JobTitle.Should().Be(updateRequest.JobTitle);
            updatedJobListing.CompanyName.Should().Be(updateRequest.CompanyName);
        }


        [Fact]
        public async Task PutJobListing_ShouldReturnNotFound_WhenJobIDIsInvalid()
        {
            // Arrange
            var invalidJobID = Guid.NewGuid();
            var updateRequest = new JobListingUpdateRequestDto
            {
                JobID = invalidJobID,
                JobTitle = "Software Engineer",
                CompanyName = "Innovative Solutions",
                Email = "careers@innovativesolutions.com",
                CityAndState = "Austin, TX",
                PayRange = "$85,000 - $125,000",
                JobType = JobTypeOptions.FullTime,
                JobPostedDate = DateTime.UtcNow,
                FullDescription = "Looking for a software engineer experienced in C# and .NET."
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"/api/JobListing/{invalidJobID}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PutJobListing_ShouldReturnBadRequest_WhenJobIdMismatch()
        {
            // Arrange
            var jobID = Guid.NewGuid();
            var updateRequest = new JobListingUpdateRequestDto
            {
                JobID = Guid.NewGuid(), //Mismatching JobID
                JobTitle = "Title",
                CompanyName = "Company",
                CityAndState = "City, State",
                FullDescription = "Description"
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync($"/api/JobListing/{jobID}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region DeleteJobListing
        [Fact]
        public async Task DeleteJobListing_ShouldReturnNoContent_WhenJobIdIsValid()
        {
            // Arrange
            var newJobListing = new JobListingAddRequestDto
            {
                JobTitle = "Test Engineer",
                CompanyName = "Tech Corp",
                Email = "hr@techcorp.com",
                CityAndState = "Los Angeles, CA",
                PayRange = "$90,000 - $110,000",
                JobType = JobTypeOptions.FullTime,
                JobPostedDate = DateTime.UtcNow,
                FullDescription = "Looking for a test engineer with experience in .NET testing."
            };

            var postResponse = await _httpClient.PostAsJsonAsync("/api/JobListing", newJobListing);
            postResponse.EnsureSuccessStatusCode(); 

            //Act
            var createdJobListing = await postResponse.Content.ReadFromJsonAsync<JobListingResponseDto>();
            var jobID = createdJobListing?.JobID;
            Assert.NotNull(jobID);

            var deleteResponse = await _httpClient.DeleteAsync($"/api/JobListing/{jobID}");

            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        [Fact]
        public async Task DeleteJobListing_ShouldReturnNotFound_WhenJobIdIsInvalid()
        {
            // Arrange
            var invalidJobID = Guid.NewGuid();

            // Act
            var response = await _httpClient.DeleteAsync($"/api/JobListing/{invalidJobID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion
    }
}
