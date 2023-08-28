using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Entities
{
    [Table("budget")]
    public class Budget
    {
        [Key]
        [Column("budget_id")]
        public int BudgetId { get; set; }
        [Column("limit_amount")]
        public decimal LimitAmount { get; set; }
        [ForeignKey(nameof(Budget))]
        [Column("user_id")]
        public int UserId { get; set; }
    }
}
