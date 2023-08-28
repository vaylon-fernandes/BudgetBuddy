using webapi.Entities;

namespace webapi.DTO
{
    public class ExpenseDTO
    {
        public int ExpenseId { get; set; }
        public string Description { get; set; } = string.Empty;
        public ExpenseCategory ExpenseCategory { get; set; }
        public decimal Amount { get; set; }
        public DateTime EnteredDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
    }
}