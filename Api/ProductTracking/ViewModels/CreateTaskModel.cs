using System.ComponentModel.DataAnnotations;

namespace ProductTracking.ViewModels
{
    public class CreateTaskModel
    {
        [Required(ErrorMessage = "Enter key words")]
        public string Keywords { get; set; }

        [Required(ErrorMessage = "Enter price")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Enter currency")]
        public string Currency { get; set; }
    }
}
