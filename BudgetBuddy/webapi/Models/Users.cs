using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("users")]
    public class Users
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("user_name")]
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Column("email")]
        
        [Required]
        public string Email { get; set; } = string.Empty;
        [Column("password")]
        [Required]
        public string Password { get; set; } = string.Empty;
        [EnumDataType(typeof(Role))]
        [Column("role")]
        public Role UserRole{ get; set; }
    }
}