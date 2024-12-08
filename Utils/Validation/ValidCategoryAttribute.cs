using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Utils.Validation
{
    public class ValidCategoryAttribute : ValidationAttribute
    {
        private readonly List<string> _validCategories = new() { "Food", "Transport", "Health", "Entertainment" };

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is string category && _validCategories.Contains(category))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid category selected.");
        }
    }
}
