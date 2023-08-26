using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webapi.Entities
{
    [Table("expenses")]
    public class Expenses
    {
        [Key]
        [Column("expense_id")]
        public int ExpenseId { get; set; }

        [Column("description", TypeName = "varchar(250)")]
        public string Description { get; set; } = string.Empty;
        [EnumDataType(typeof(Role))]
        [Column("expense_category", TypeName = "varchar(25)")]
        public ExpenseCategory ExpenseCategory { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [ForeignKey("user_id")]
        public int UserId { get; set; }
       // [JsonIgnore]
        public Users User { get; set; }
    }
}
