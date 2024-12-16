using ExpenseTracker.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [ValidAmount]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidDateNotInPreviousMonth]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [ValidCategory]
        [Display(Name = "Category")]
        public required string Category { get; set; }

        public string? UserId { get; set; }
    }
}
