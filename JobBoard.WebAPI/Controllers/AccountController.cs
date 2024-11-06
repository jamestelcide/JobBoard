using JobBoard.Core.Domain.IdentityEntities;
using JobBoard.Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">Manages user-related operations.</param>
        /// <param name="signInManager">Manages user sign-in operations.</param>
        /// <param name="roleManager">Manages role-related operations.</param>
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="registerDto">The registration details.</param>
        /// <returns>A newly created user if successful; otherwise, an error message.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDto registerDto)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
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
                return Ok(user);
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        /// <summary>
        /// Checks if an email address is already registered.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email is not registered; false otherwise.</returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyREgistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Ok(true);
            }
            else
            {
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
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return NoContent();
                }

                return Ok(new { personName = user.PersonName, email = user.Email });
            }
            else
            {
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
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
