using System.ComponentModel.DataAnnotations;

namespace JobBoard.Core.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email can not be blank!")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can not be blank!")]
        public string Password { get; set; } = string.Empty;
    }
}
