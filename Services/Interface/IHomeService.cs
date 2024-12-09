using ExpenseTracker.Models.ViewModels.HomeViewModels;

namespace ExpenseTracker.Services.Interface
{
    public interface IHomeService
    {
        Task<HomeIndexViewModel> GetHomeIndexDataAsync(string userId);
        decimal CalculateRemainingOverdraftLimit(decimal balance, decimal allowedOverdraftLimit);
    }
}
