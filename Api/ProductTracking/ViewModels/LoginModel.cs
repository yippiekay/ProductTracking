using System.ComponentModel.DataAnnotations;

namespace ProductTracking.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
