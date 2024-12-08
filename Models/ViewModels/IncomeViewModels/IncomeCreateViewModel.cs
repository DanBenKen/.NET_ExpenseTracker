using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.ViewModels.IncomeViewModels
{
    public class IncomeCreateViewModel
    {
        [Required]
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
