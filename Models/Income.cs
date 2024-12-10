using ExpenseTracker.Utils.Validation;

namespace ExpenseTracker.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string? Description { get; set; }
    }
}
