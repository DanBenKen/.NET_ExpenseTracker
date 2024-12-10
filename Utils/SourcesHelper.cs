using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Utils
{
    public class SourcesHelper
    {
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
