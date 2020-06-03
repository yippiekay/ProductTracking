using System.Collections.Generic;

namespace ProductTracking.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
 
        public string Role { get; set; }

        public List<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();
    }
}
