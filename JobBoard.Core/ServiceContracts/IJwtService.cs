using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Dto;

namespace JobBoard.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}
