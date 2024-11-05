using AutoFixture;
using JobBoard.Core.ServiceContracts;
using Moq;

namespace JobBoard.ControllerTests
{
    public class JobListingControllerTest
    {
        private readonly IJobListingService _jobListingService;
        private readonly Mock<IJobListingService> _jobListingServiceMock;
        private readonly Fixture _fixture;

        public JobListingControllerTest()
        {
            _fixture = new Fixture();
            _jobListingServiceMock = new Mock<IJobListingService>();
            _jobListingService = _jobListingServiceMock.Object;
        }

        #region Index
        #endregion

        #region Create
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion
    }
}
