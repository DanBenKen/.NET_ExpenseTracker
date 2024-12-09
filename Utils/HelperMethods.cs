using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Utils
{
    public class HelperMethods
    {
        public List<int> pageSizeList = new List<int> { 5, 10, 25, 50 };

        public List<SelectListItem> GetMonths()
        {
            return Enumerable.Range(1, 12)
                .Select(m => new SelectListItem
                {
                    Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m),
                    Value = m.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetCategories()
        {
            return new List<SelectListItem>{
                new SelectListItem { Text = "All Categories", Value = "all_categories" },
                new SelectListItem { Text = "Food", Value = "food" },
                new SelectListItem { Text = "Transport", Value = "transport" },
                new SelectListItem { Text = "Health", Value = "health" },
                new SelectListItem { Text = "Entertainment", Value = "entertainment" },
                new SelectListItem { Text = "Utilities", Value = "utilities" },
                new SelectListItem { Text = "Housing", Value = "housing" },
                new SelectListItem { Text = "Education", Value = "education" },
                new SelectListItem { Text = "Clothing", Value = "clothing" },
                new SelectListItem { Text = "Travel", Value = "travel" },
                new SelectListItem { Text = "Gifts & Donations", Value = "gifts_donations" },
                new SelectListItem { Text = "Personal Care", Value = "personal_care" },
                new SelectListItem { Text = "Insurance", Value = "insurance" }
            };
        }

        public List<SelectListItem> GetSources()
        {
            return new List<SelectListItem>{
                new SelectListItem { Text = "All Sources", Value = "all_sources" },
                new SelectListItem { Text = "Cash", Value = "cash" },
                new SelectListItem { Text = "Bank Transfer", Value = "bank_transfer" },
                new SelectListItem { Text = "Credit Card", Value = "credit_card" },
                new SelectListItem { Text = "PayPal", Value = "paypal" },
                new SelectListItem { Text = "Cryptocurrency", Value = "cryptocurrency" },
                new SelectListItem { Text = "Mobile Payment", Value = "mobile_payment" }
            };
        }
    }
}
