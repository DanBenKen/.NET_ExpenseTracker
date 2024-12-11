using ExpenseTracker.Models.ViewModels.IncomeViewModels;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<IncomeIndexViewModel> GetIncomesViewModelAsync(string userId, int? month, int? year, string? source, int pageNumber, int pageSize, bool showAll);
        IncomeCreateViewModel GetCreateIncomeViewModel(string userId);
        Task<IncomeCreateViewModel> GetIncomeForEditAsync(int id, string userId);
        Task<bool> CreateIncomeAsync(IncomeCreateViewModel viewModel, string userId);
        Task<bool> UpdateIncomeAsync(int id, string userId, IncomeCreateViewModel viewModel);
        Task<bool> DeleteIncomeAsync(int id, string userId);
    }
}
