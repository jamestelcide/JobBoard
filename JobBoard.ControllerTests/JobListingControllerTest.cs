using AutoFixture;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using JobBoard.Core.Services;
using JobBoard.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JobBoard.ControllerTests
{
    public class JobListingControllerTest
    {
        private readonly IJobListingService _jobListingService;
        private readonly Mock<IJobListingService> _jobListingServiceMock;
        private readonly IJobListingRepository _jobListingRepository;
        private readonly Mock<IJobListingRepository> _jobListingRepositoryMock;
        private readonly JobListingController _jobListingController;
        private readonly Fixture _fixture;

        public JobListingControllerTest()
        {
            _fixture = new Fixture();
            _jobListingServiceMock = new Mock<IJobListingService>();
            _jobListingService = _jobListingServiceMock.Object;
            _jobListingRepositoryMock = new Mock<IJobListingRepository>();
            _jobListingRepository = _jobListingRepositoryMock.Object;
            var loggerMock = new Mock<ILogger<JobListingController>>();
            _jobListingController = new JobListingController(_jobListingServiceMock.Object,
                _jobListingRepositoryMock.Object, loggerMock.Object);
        }

        #region GetJobListings
        [Fact]
        public async Task GetJobListings_ShouldReturnOkResult_WithListOfJobListings()
        {
            // Arrange
            var jobListingResponseList = _fixture.CreateMany<JobListingResponseDto>(3).ToList();
            _jobListingServiceMock.Setup(service => service.GetAllJobListingsAsync())
            .ReturnsAsync(jobListingResponseList);

            // Act
            var result = await _jobListingController.GetJobListings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedJobListings = Assert.IsAssignableFrom<IEnumerable<JobListingResponseDto>>(okResult.Value);
            Assert.Equal(3, returnedJobListings.Count());
        }

        [Fact]
        public async Task GetJobListings_ShouldReturnOkResult_WithEmptyList_WhenNoJobListingsExist()
        {
            // Arrange
            var emptyJobListingResponseList = new List<JobListingResponseDto>();
            _jobListingServiceMock.Setup(service => service.GetAllJobListingsAsync())
            .ReturnsAsync(emptyJobListingResponseList);

            // Act
            var result = await _jobListingController.GetJobListings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedJobListings = Assert.IsAssignableFrom<IEnumerable<JobListingResponseDto>>(okResult.Value);
            Assert.Empty(returnedJobListings);
        }
        #endregion

        #region GetJobListingsByCityAndState
        [Fact]
        public async Task GetJobListingsByCityAndState_ShouldReturnOkResult_WithOnlyMatchingJobListings()
        {
            // Arrange
            string targetCityAndState = "Atlanta, GA";
            
            var jobListings = _fixture.CreateMany<JobListingResponseDto>(5).ToList();

            jobListings[0].CityAndState = targetCityAndState;
            jobListings[1].CityAndState = targetCityAndState;
            jobListings[2].CityAndState = "New York, NY";
            jobListings[3].CityAndState = "Los Angeles, CA";
            jobListings[4].CityAndState = targetCityAndState;

            var matchingJobListings = jobListings.Where(j => j.CityAndState == targetCityAndState).ToList();
            _jobListingServiceMock
                .Setup(service => service.GetJobListingsByCityAndState(targetCityAndState))
                .ReturnsAsync(matchingJobListings);

            // Act
            var result = await _jobListingController.GetJobListingsByCityAndState(targetCityAndState);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedJobListings = Assert.IsAssignableFrom<IEnumerable<JobListingResponseDto>>(okResult.Value);
            Assert.Equal(3, returnedJobListings.Count()); // Only 3 job listings match the target city and state

            Assert.All(returnedJobListings, j => Assert.Equal(targetCityAndState, j.CityAndState));
        }

        [Fact]
        public async Task GetJobListingsByCityAndState_ShouldReturnNotFound_WhenNoJobListingsExist()
        {
            // Arrange
            string cityAndState = "Nonexistent City, XY";
            _jobListingServiceMock
                .Setup(service => service.GetJobListingsByCityAndState(cityAndState))
                .ReturnsAsync(new List<JobListingResponseDto>());

            // Act
            var result = await _jobListingController.GetJobListingsByCityAndState(cityAndState);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No match found for CityAndState.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetJobListingsByCityAndState_ShouldReturnNotFound_WhenNoMatchingJobListingsExist()
        {
            // Arrange
            string targetCityAndState = "Atlanta, GA";

            var jobListings = _fixture.CreateMany<JobListingResponseDto>(5).ToList();
            jobListings.ForEach(j => j.CityAndState = "Different City, DC");

            _jobListingServiceMock
                .Setup(service => service.GetJobListingsByCityAndState(targetCityAndState))
                .ReturnsAsync(new List<JobListingResponseDto>()); 

            // Act
            var result = await _jobListingController.GetJobListingsByCityAndState(targetCityAndState);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No match found for CityAndState.", notFoundResult.Value);
        }
        #endregion

        #region PutJobListing
        [Fact]
        public async Task PutJobListing_ShouldReturnBadRequest_WhenJobIDDoesNotMatch()
        {
            // Arrange
            Guid urlJobID = Guid.NewGuid();
            var jobListingUpdateRequest = _fixture.Create<JobListingUpdateRequestDto>();
            jobListingUpdateRequest.JobID = Guid.NewGuid(); // Different ID from the URL

            // Act
            var result = await _jobListingController.PutJobListing(urlJobID, jobListingUpdateRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("JobID in the URL does not match JobID in the body.", badRequestResult.Value);
        }

        [Fact]
        public async Task PutJobListing_ShouldReturnOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var jobID = Guid.NewGuid();
            var jobListingUpdateRequest = _fixture.Create<JobListingUpdateRequestDto>();
            jobListingUpdateRequest.JobID = jobID;

            var updatedJobListing = _fixture.Create<JobListingResponseDto>();

            _jobListingServiceMock
                .Setup(service => service.UpdateJobListingAsync(jobListingUpdateRequest))
                .ReturnsAsync(updatedJobListing);

            // Act
            var result = await _jobListingController.PutJobListing(jobID, jobListingUpdateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedJobListing, okResult.Value);
        }

        [Fact]
        public async Task PutJobListing_ShouldReturnNotFound_WhenJobListingDoesNotExist()
        {
            // Arrange
            var jobID = Guid.NewGuid();
            var jobListingUpdateRequest = _fixture.Create<JobListingUpdateRequestDto>();
            jobListingUpdateRequest.JobID = jobID;

            _jobListingServiceMock
                .Setup(service => service.UpdateJobListingAsync(jobListingUpdateRequest))
                .ThrowsAsync(new ArgumentNullException());

            // Act
            var result = await _jobListingController.PutJobListing(jobID, jobListingUpdateRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Value cannot be null.", notFoundResult.Value);
        }
        #endregion

        #region PostJobListing
        [Fact]
        public async Task PostJobListing_ShouldReturnCreatedAtActionResult_WhenCreationIsSuccessful()
        {
            // Arrange
            var jobListingAddRequest = _fixture.Create<JobListingAddRequestDto>();
            var createdJobListing = _fixture.Create<JobListingResponseDto>();
            createdJobListing.CityAndState = jobListingAddRequest.CityAndState;

            _jobListingServiceMock
                .Setup(service => service.AddJobListingAsync(jobListingAddRequest))
                .ReturnsAsync(createdJobListing);

            // Act
            var result = await _jobListingController.PostJobListing(jobListingAddRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_jobListingController.GetJobListingsByCityAndState), createdAtActionResult.ActionName);
            Assert.Equal(createdJobListing, createdAtActionResult.Value);
            Assert.Equal(createdJobListing.CityAndState, ((JobListingResponseDto)createdAtActionResult.Value!).CityAndState);
        }
        #endregion

        #region DeleteJobListing
        [Fact]
        public async Task DeleteJobListing_ShouldReturnNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            var jobId = Guid.NewGuid();

            _jobListingServiceMock
                .Setup(service => service.DeleteJobListingAsync(jobId))
                .ReturnsAsync(true);

            // Act
            var result = await _jobListingController.DeleteJobListing(jobId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteJobListing_ShouldReturnNotFound_WhenDeletionFails()
        {
            // Arrange
            var jobId = Guid.NewGuid();

            _jobListingServiceMock
                .Setup(service => service.DeleteJobListingAsync(jobId))
                .ReturnsAsync(false);

            // Act
            var result = await _jobListingController.DeleteJobListing(jobId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("JobListing not found.", notFoundResult.Value);
        }
        #endregion
    }
}
