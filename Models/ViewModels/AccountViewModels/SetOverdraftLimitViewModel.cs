using ExpenseTracker.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.ViewModels.AccountViewModels
{
    public class SetOverdraftLimitViewModel
    {
        [Required]
        [Display(Name = "Allowed Overdraft Limit")]
        [DataType(DataType.Currency)]
        [ValidExpenseAmount]
        public decimal AllowedOverdraftLimit { get; set; }
    }
}
