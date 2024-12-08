using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Utils.Validation
{
    public class ValidExpenseAmountAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal amount)
            {
                if (amount < 0)
                {
                    return new ValidationResult("Amount must be greater than or equal to 0.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
