using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Utils
{
    public class MonthsHelper
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
    }
}
