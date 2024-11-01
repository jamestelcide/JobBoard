using JobBoard.Core.Domain.RepositoryContracts;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;

namespace JobBoard.Core.Services
{
    public class JobListingService : IJobListingService
    {
        private readonly IJobListingRepository _jobListingRepository;

        public JobListingService(IJobListingRepository jobListingRepository) 
        {
            _jobListingRepository = jobListingRepository;
        }

        public Task<JobListingResponseDto> AddJobListingAsync(JobListingAddRequestDto? jobListingAddRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobListingResponseDto>> GetAllJobListingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<JobListingResponseDto?>> GetJobListingsByCityAndState(string? CityAndState)
        {
            throw new NotImplementedException();
        }

        public Task<JobListingResponseDto> UpdateJobListingAsync(JobListingUpdateRequestDto? JobListingUpdateRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteJobListingAsync(Guid JobID)
        {
            throw new NotImplementedException();
        }

    }
}
