﻿namespace ProductTracking.DAL.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Keywords { get; set; }

        public int Price { get; set; }

        public string Currency { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
