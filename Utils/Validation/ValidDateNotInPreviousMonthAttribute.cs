using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Utils.Validation
{
    public class ValidDateNotInPreviousMonthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date is required.");

            if (value is DateTime dateValue)
            {
                var now = DateTime.Now;
                DateTime firstDayOfPreviousMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                DateTime lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);

                if (dateValue.Date >= firstDayOfPreviousMonth.Date && dateValue.Date <= lastDayOfPreviousMonth.Date)
                {
                    return new ValidationResult("The date cannot be in the previous month.");
                }
            }

            return ValidationResult.Success;
        }
    }
}