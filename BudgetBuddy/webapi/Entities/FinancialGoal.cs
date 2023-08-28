using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Entities
{
    
    
        [Table("financial_goals")]
        public class FinancialGoal
        {
            [Key]
            [Column("goal_id")]
            public int GoalId { get; set; }

            [Column("goal_name", TypeName = "varchar(250)")]
            [Required]
            public string GoalName { get; set; } = string.Empty;

            [Column("amount", TypeName = "decimal(18, 2)")]
            [Required]
            public decimal Amount { get; set; }

            [Column("user_id")]
            [Required]  
            public int UserId { get; set; }
        }
}
