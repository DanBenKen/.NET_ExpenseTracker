namespace ExpenseTracker.Models.ViewModels.HomeViewModels
{
    public class HomeIndexViewModel
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AllowedOverdraftLimit { get; set; }
        public decimal RemainingOverdraftLimit { get; set; }
    }
}
