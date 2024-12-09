using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Models.ViewModels.IncomeViewModels
{
    public class IncomeIndexViewModel
    {
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
        public string SelectedSource { get; set; }
        public bool ShowAll { get; set; }

        public List<Income> Incomes { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> Sources { get; set; }
        public PaginatedList<Income> PaginatedItems { get; set; }
    }
}
