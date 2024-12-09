using ExpenseTracker.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.ViewModels.AccountViewModels
{
    public class SetOverdraftLimitViewModel
    {
        [Required]
        [Display(Name = "Allowed Overdraft Limit")]
        [DataType(DataType.Currency)]
        [ValidAmount]
        public decimal AllowedOverdraftLimit { get; set; }
    }
}
