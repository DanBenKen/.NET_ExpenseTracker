using ExpenseTracker.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a description for the expense.")]
        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [ValidExpenseAmount]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidExpenseDate]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [ValidCategory]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        public string UserId { get; set; }
    }
}
