using ExpenseTracker.Utils.Validation;

namespace ExpenseTracker.Models
{
    public class Income
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public required string Source { get; set; }
        public string? Description { get; set; }
    }
}
