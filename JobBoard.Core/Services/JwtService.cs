using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JobBoard.Core.Services
{
    public class JwtService : IJwtService
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
            
            if (string.IsNullOrEmpty(user.Email))
            {
                _logger.LogError("User email is null or empty.");
                throw new ArgumentNullException(nameof(user.Email), "User email is required.");
            }

            if (string.IsNullOrEmpty(user.PersonName))
            {
                _logger.LogError("User person name is null or empty.");
                throw new ArgumentNullException(nameof(user.PersonName), "User person name is required.");
            }

            Claim[] claims = new Claim[] {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Email),
        new Claim(ClaimTypes.Name, user.PersonName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            string jwtKey = _configuration["Jwt:Key"]!;
            if (string.IsNullOrEmpty(jwtKey))
            {
                _logger.LogError("JWT key is missing in configuration.");
                throw new ArgumentNullException("Jwt:Key is required in the configuration.");
            }

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            _logger.LogDebug("Security key created successfully.");

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
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
            };
        }

        private string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),

                ValidateLifetime = false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
