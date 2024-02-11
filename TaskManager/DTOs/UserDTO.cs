using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTOs
{
    public class UserDTO
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
