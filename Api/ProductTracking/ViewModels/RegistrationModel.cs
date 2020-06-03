using System.ComponentModel.DataAnnotations;

namespace ProductTracking.ViewModels
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't equals")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Enter role")]
        public string Role { get; set; }
    }
}
