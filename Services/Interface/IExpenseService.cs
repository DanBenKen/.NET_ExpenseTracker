using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using System.Security.Claims;

namespace ExpenseTracker.Services.Interface
{
    public interface IExpenseService
    {
        Task<ExpenseIndexViewModel> GetExpensesAsync(ClaimsPrincipal user, int? month, int? year, string? category, int pageNumber, int pageSize, bool showAll);
        Task<bool> CreateExpenseAsync(ExpenseCreateEditViewModel viewModel, ClaimsPrincipal user);
        Task<bool> DeleteExpenseAsync(int id, ClaimsPrincipal user);
        Task<ExpenseCreateEditViewModel?> GetExpenseByIdAsync(int id, ClaimsPrincipal user);
        Task<bool> EditExpenseAsync(ExpenseCreateEditViewModel model, ClaimsPrincipal user);
    }
}
