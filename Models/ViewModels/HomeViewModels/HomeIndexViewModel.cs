namespace ExpenseTracker.Models.ViewModels.HomeViewModels
{
    public class HomeIndexViewModel
    {
        public decimal ExpensesAmount { get; set; }
        public decimal IncomesAmount { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AllowedOverdraftLimit { get; set; }
        public decimal RemainingOverdraftLimit { get; set; }
    }
}
