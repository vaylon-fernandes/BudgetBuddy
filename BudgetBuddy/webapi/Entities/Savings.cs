using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Entities
{
    [Table("savings")]
    public class Savings
    {
        [Key]
        [Column("savings_id")]
        public int SavingsId { get; set; }

        [Column("amount", TypeName = "decimal(18, 2)")]
        [Required]
        public decimal Amount { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        [Required]  // One-to-one relation requires a required foreign key
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
