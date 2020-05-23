using System.ComponentModel.DataAnnotations;

namespace ProductTracking.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Укажите имя пользователя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        
        public string Role { get; set; }
    }
}
