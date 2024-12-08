using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Utils.Validation
{
    public class ValidExpenseDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue.Date < DateTime.Now.Date)
                {
                    return new ValidationResult("The date cannot be in the past.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
