using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column("amount")]
        public decimal Amount { get; set; }

        [ForeignKey("user_id")]
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
