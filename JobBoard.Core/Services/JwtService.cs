using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoard.Core.Services
{
    internal class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            _logger.LogInformation("Starting JWT token creation for user {UserId}", user.Id);

            DateTime expiration;
            try
            {
                expiration = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"])
                );
                _logger.LogDebug("Token expiration set to {Expiration}", expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse expiration time from configuration.");
                throw;
            }

            Claim[] claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.PersonName)
            };

            SymmetricSecurityKey securityKey;
            try
            {
                securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );
                _logger.LogDebug("Security key created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create security key from configuration.");
                throw;
            }

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            _logger.LogInformation("JWT token created successfully for user {UserId}", user.Id);

            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                PersonName = user.PersonName,
                Expiration = expiration
            };
        }
    }
}
