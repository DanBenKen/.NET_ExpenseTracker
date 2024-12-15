using ExpenseTracker.Models.ViewModels.IncomeViewModels;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<IncomeIndexViewModel?> GetIncomesViewModelAsync(string userId, int? month, int? year, string? source, int pageNumber, int pageSize, bool showAll);
        IncomeCreateEditViewModel GetCreateIncomeViewModel(string userId);
        Task<IncomeCreateEditViewModel?> GetIncomeForEditAsync(int id, string userId);
        Task<bool> CreateIncomeAsync(IncomeCreateEditViewModel viewModel, string userId);
        Task<bool> UpdateIncomeAsync(int id, string userId, IncomeCreateEditViewModel viewModel);
        Task<bool> DeleteIncomeAsync(int id, string userId);
    }
}
