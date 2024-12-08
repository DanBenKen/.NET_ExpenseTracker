using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Models.ViewModels.IncomeViewModels
{
    public class IncomeViewModel
    {
        public List<Income> Incomes { get; set; }
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
        public string SelectedSource { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> Sources { get; set; }
    }
}
