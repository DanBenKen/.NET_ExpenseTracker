using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Models.ViewModels.ExpenseViewModels
{
    public class ExpenseIndexViewModel
    {
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
        public string SelectedCategory { get; set; }

        public List<Expense> Expenses { get; set; }
        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}