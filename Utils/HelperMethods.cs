using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Utils
{
    public class HelperMethods
    {
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
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "All Categories", Value = "AllCategories" },
                new SelectListItem { Text = "Food", Value = "Food" },
                new SelectListItem { Text = "Transport", Value = "Transport" },
                new SelectListItem { Text = "Health", Value = "Health" },
                new SelectListItem { Text = "Entertainment", Value = "Entertainment" }
            };
        }

        public List<SelectListItem> GetSources()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "All Sources", Value = "AllSources" },
                new SelectListItem { Text = "Salary", Value = "Salary" },
                new SelectListItem { Text = "Passive Income", Value = "PassiveIncome" },
            };
        }
    }
}
