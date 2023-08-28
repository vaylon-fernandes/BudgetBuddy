using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;


namespace webapi.Entities
{
    [Table("users")]
    public class Users
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("user_name",TypeName = "varchar(250)")]
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Column("email",TypeName= "varchar(100)")]
        [Required]
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        [Column("password",TypeName ="varchar(255)")]
        public string Password { get; set; } = string.Empty;
        [EnumDataType(typeof(Role))]
        [Column("role",TypeName="varchar(10)")]
        public Role UserRole{ get; set; }
        [Column("salt", TypeName = "varchar(50)")]
        public string Salt { get; set; } = string.Empty;
        public List<Expenses>? Expenses { get; set; }
        public Budget? Budget { get; set; }
        public List<FinancialGoal>? FinancialGoals { get; set; }

    }
}