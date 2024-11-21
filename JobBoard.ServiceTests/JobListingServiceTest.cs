using AutoFixture;
using FluentAssertions;
using JobBoard.Core.Domain.Entities;
using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using JobBoard.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace JobBoard.ServiceTests
{
    public class JobListingServiceTest
    {
        private readonly IJobListingService _jobListingService;
        private readonly Mock<IJobListingRepository> _jobListingRepositoryMock;
        private readonly IJobListingRepository _jobListingRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;


        public JobListingServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _jobListingRepositoryMock = new Mock<IJobListingRepository>();
            _jobListingRepository = _jobListingRepositoryMock.Object;
            var loggerMock = new Mock<ILogger<JobListingService>>();
            _jobListingService = new JobListingService(_jobListingRepository, loggerMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        #region AddJobListingAsync
        [Fact]
        public async Task AddJobListingAsync_NullJobListingAddRequest_ShouldThrowArgumentNullException()
        {
            //Arrange
            JobListingAddRequestDto jobListingAddRequest = null!;

            //Act
            Func<Task> action = async () => { await _jobListingService.AddJobListingAsync(jobListingAddRequest); };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddJobListingAsync_FullDetails_ToBeSuccessful()
        {
            //Arrange
            JobListingAddRequestDto? jobListingAddRequest = 
                _fixture.Build<JobListingAddRequestDto>()
                .Create();

            JobListing joblisting = jobListingAddRequest.ToJobListing();
            JobListingResponseDto expectedJobListingResponse = joblisting.ToJobListingResponse();

            _jobListingRepositoryMock.Setup(temp => temp.AddJobListingAsync(It.IsAny<JobListing>()))
                .ReturnsAsync(joblisting);

            //Act
            JobListingResponseDto jobListingResponseFromAdd = await 
                _jobListingService.AddJobListingAsync(jobListingAddRequest);

            expectedJobListingResponse.JobID = jobListingResponseFromAdd.JobID;

            //Assert
            jobListingResponseFromAdd.Should().NotBe(Guid.Empty);
            jobListingResponseFromAdd.Should().Be(expectedJobListingResponse);
        }
        #endregion

        #region GetAllJobListingsAsync
        [Fact]
        public async Task GetAllJobListingsAsync_ToBeEmptyList()
        {
            //Arrange
            var jobListings = new List<JobListing>();
            
            _jobListingRepositoryMock
                .Setup(j => j.GetAllJobListingsAsync())
                .ReturnsAsync(jobListings);

            //Act
            List<JobListingResponseDto> jobListingFromGet = await _jobListingService.GetAllJobListingsAsync();

            //Assert
            jobListingFromGet.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllJobListingsAsync_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
            List<JobListing> jobListings = new List<JobListing>()
            {
                _fixture.Build<JobListing>().Create(),
                _fixture.Build<JobListing>().Create(),
                _fixture.Build<JobListing>().Create()
            };

            List<JobListingResponseDto> expectedJobListingResponseList = 
                jobListings.Select(j => j.ToJobListingResponse()).ToList();

            _testOutputHelper.WriteLine("Expected: ");
            foreach (JobListingResponseDto jobListingResponseFromAdd in expectedJobListingResponseList) 
            {
                _testOutputHelper.WriteLine(jobListingResponseFromAdd.ToString());
            }

            _jobListingRepositoryMock.Setup(j => j.GetAllJobListingsAsync()).ReturnsAsync(jobListings);

            //Act
            List<JobListingResponseDto> jobListingFromGet = await _jobListingService.GetAllJobListingsAsync();

            _testOutputHelper.WriteLine("Actual: ");
            foreach (JobListingResponseDto jobListingResponseFromGet in jobListingFromGet)
            {
                _testOutputHelper.WriteLine(jobListingResponseFromGet.ToString());
            }

            //Assert
            jobListingFromGet.Should().BeEquivalentTo(expectedJobListingResponseList);
        }

        #endregion

        #region GetJobListingByIDAsync
        [Fact]
        public async Task GetJobListingByIDAsync_ShouldReturnNull_WhenJobIDIsNull()
        {
            // Arrange
            Guid? jobID = null;

            // Act
            var result = await _jobListingService.GetJobListingByIDAsync(jobID);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetJobListingByIDAsync_ShouldReturnJobListingResponseDto_WhenJobListingFound()
        {
            // Arrange
            var jobID = Guid.NewGuid();
            var jobListing = _fixture.Build<JobListing>()
                .With(j => j.JobID, jobID)
                .With(j => j.JobTitle, "Software Engineer")
                .With(j => j.CompanyName, "Tech Corp")
                .Create();

            var expectedJobListingResponse = jobListing.ToJobListingResponse();

            _jobListingRepositoryMock.Setup(j => j.GetJobListingByJobID(jobID))
                .ReturnsAsync(jobListing);

            // Act
            var result = await _jobListingService.GetJobListingByIDAsync(jobID);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedJobListingResponse, options => options.ExcludingMissingMembers());
        }
        #endregion

        #region GetJobListingsByCityAndState
        [Fact]
        public async Task GetJobListingsByCityAndState_WithCityAndState_ToBeSuccessful()
        {
            // Arrange
            var jobListings = _fixture.Build<JobListing>()
                .With(j => j.CityAndState, "Atlanta, GA")
                .CreateMany(3)
                .ToList();

            List<JobListingResponseDto> jobListingResponsesExpected = jobListings
                .Select(j => j.ToJobListingResponse())
                .ToList();

            _jobListingRepositoryMock.Setup(j => j.GetJobListingsByCityAndStateAsync(It.IsAny<string>()))
                .ReturnsAsync(jobListings);

            // Act
            List<JobListingResponseDto> jobListingResponsesFromGet = await
                _jobListingService.GetJobListingsByCityAndState("Atlanta, GA");

            // Assert
            jobListingResponsesFromGet.Should().BeEquivalentTo(jobListingResponsesExpected);
        }
        #endregion

        #region UpdateJobListingAsync
        [Fact]
        public async Task UpdateJobListingAsync_NullJobListing_ToBeArgumentNullException()
        {
            //Arrange
            JobListingUpdateRequestDto? jobListingUpdateRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _jobListingService.UpdateJobListingAsync(jobListingUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateJobListingAsync_InvalidJobID_ToBeArgumentException()
        {
            //Arrange
            JobListingUpdateRequestDto? jobListingUpdateRequest = 
                _fixture.Build<JobListingUpdateRequestDto>().Create();

            //Act
            Func<Task> action = async () =>
            {
                await _jobListingService.UpdateJobListingAsync(jobListingUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }
        #endregion

        #region DeleteJobListingAsync
        [Fact]
        public async Task DeleteJobListingAsync_InvalidJobID()
        {
            //Act
            bool isDeleted = await _jobListingService.DeleteJobListingAsync(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteJobListingAsync_ValidJobID_ToBeSuccessful()
        {
            //Arrange
            JobListing jobListing = _fixture.Build<JobListing>().Create();

            _jobListingRepositoryMock.Setup(j => j.DeleteJobListingByIDAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _jobListingRepositoryMock.Setup(j => j.GetJobListingByJobID(It.IsAny<Guid>()))
                .ReturnsAsync(jobListing);

            //Act
            bool isDeleted = await _jobListingService.DeleteJobListingAsync(jobListing.JobID);

            //Assert
            isDeleted.Should().BeTrue();
        }
        #endregion
    }
}
