using System.Collections.Generic;

namespace ProductTracking.DAL.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
