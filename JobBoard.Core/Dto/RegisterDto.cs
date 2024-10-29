using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Core.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Person Name can not be blank!")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email can not be blank!")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format!")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already in use!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber can not be blank!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain digits only!")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can not be blank!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password can not be blank!")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
