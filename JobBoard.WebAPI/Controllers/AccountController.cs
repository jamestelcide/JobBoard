using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Dto;
using JobBoard.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobBoard.WebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing user accounts, including registration, login, and logout functionalities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Initializes a new instance of the AccountController class.
        /// </summary>
        /// <param name="userManager">Manages user-related operations.</param>
        /// <param name="signInManager">Manages user sign-in operations.</param>
        /// <param name="roleManager">Manages role-related operations.</param>
        /// <param name="logger">Logger for logging events and errors.</param>
        /// <param name="jwtService">The service responsible for handling JWT authentication operations.</param>
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<AccountController> logger, 
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="registerDto">The registration details.</param>
        /// <returns>A newly created user if successful; otherwise, an error message.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDto registerDto)
        {
            _logger.LogInformation("Attempting to register a new user with email: {Email}", registerDto.Email);

            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Registration failed due to invalid model state: {Errors}", errorMessage);
                return Problem(errorMessage);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email,
                PersonName = registerDto.PersonName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                var authenticationResponse = _jwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation("User sign in successful for email: {Email}", registerDto.Email);
                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                _logger.LogError("User sign in failed for email: {Email} with errors: {Errors}", registerDto.Email, errorMessage);
                return Problem(errorMessage);
            }
        }

        /// <summary>
        /// Checks if an email address is already registered.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email is not registered; false otherwise.</returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            _logger.LogInformation("Checking if email is already registered: {Email}", email);

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation("Email {Email} is not registered", email);
                return Ok(true);
            }
            else
            {
                _logger.LogInformation("Email {Email} is already registered", email);
                return Ok(false);
            }
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="loginDto">The login details.</param>
        /// <returns>Information about the logged-in user if successful; otherwise, an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(LoginDto loginDto)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", loginDto.Email);

            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Login failed due to invalid model state: {Errors}", errorMessage);
                return Problem(errorMessage);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: no user found for email {Email} after successful sign-in", loginDto.Email);
                    return NoContent();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(user);

                user.RefreshToken = authenticationResponse.RefreshToken;

                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation("User login successful for email: {Email}", loginDto.Email);
                return Ok(new { personName = user.PersonName, email = user.Email });
            }
            else
            {
                _logger.LogWarning("Login failed for email: {Email} due to invalid credentials", loginDto.Email);
                return Problem("Invalid Email or Password");
            }
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>No content if successful.</returns>
        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            _logger.LogInformation("Attempting to log out the current user");

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out successfully");
            return NoContent();
        }

        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> GenerateNewAccessToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request");
            }

            ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token);
            if (principal == null)
            {
                return BadRequest("Invalid jwt access token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser? user = await _userManager.FindByNameAsync(email);

            if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.Now)
            {
                return BadRequest("Invalid refresh token");
            }

            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }
    }
}
