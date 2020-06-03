﻿using System.Collections.Generic;

namespace ProductTracking.BLL.Models
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public List<TaskDTO> Tasks { get; set; }
    }
}
