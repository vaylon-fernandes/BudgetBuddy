using webapi.Entities;

namespace webapi.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role UserRole { get; set; }

    }
   
}
