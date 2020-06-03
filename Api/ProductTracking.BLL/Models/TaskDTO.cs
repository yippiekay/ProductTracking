namespace ProductTracking.BLL.Models
{
    public class TaskDTO
    {
        public int Id { get; set; }

        public string Keywords { get; set; }

        public int Price { get; set; }

        public string Currency { get; set; }

        public UserDTO User { get; set; }
    }
}
