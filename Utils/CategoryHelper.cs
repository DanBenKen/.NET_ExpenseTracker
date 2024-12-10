using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Utils
{
    public class CategoryHelper
    {
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
    }
}
