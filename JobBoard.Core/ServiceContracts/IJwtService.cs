using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Dto;
using System.Security.Claims;

namespace JobBoard.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
