using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Models.ViewModels.ExpenseViewModels
{
    public class ExpenseIndexViewModel
    {
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
        public required string SelectedCategory { get; set; }
        public bool ShowAll { get; set; }
        public int PageSize { get; set; } = 5;

        public List<int> PageSizeOptions { get; set; } = new List<int> { 5, 10, 25, 50 };
        public required List<Expense> Expenses { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public PaginatedList<Expense>? PaginatedItems { get; set; }
    }
}