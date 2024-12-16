using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Models.ViewModels.IncomeViewModels
{
    public class IncomeIndexViewModel
    {
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
        public string? SelectedSource { get; set; }
        public bool ShowAll { get; set; }
        public int PageSize { get; set; } = 5;

        public List<int> PageSizeOptions { get; set; } = new List<int> { 5, 10, 25, 50 };
        public required List<Income> Incomes { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
        public IEnumerable<SelectListItem>? Sources { get; set; }
        public PaginatedList<Income>? PaginatedItems { get; set; }
    }
}
