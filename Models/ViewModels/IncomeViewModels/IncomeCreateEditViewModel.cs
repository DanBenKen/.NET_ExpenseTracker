using ExpenseTracker.Utils.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.ViewModels.IncomeViewModels
{
    public class IncomeCreateEditViewModel
    {
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        [ValidAmount]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date")]
        [ValidDateNotInPreviousMonth]
        public DateTime Date { get; set; }

        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Source")]
        public required string Source { get; set; }

        public List<SelectListItem> Sources { get; set; } = new List<SelectListItem>();
    }
}
