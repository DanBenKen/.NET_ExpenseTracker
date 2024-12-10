﻿using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using System.Security.Claims;

namespace ExpenseTracker.Services.Interface
{
    public interface IExpenseService
    {
        Task<ExpenseIndexViewModel> GetExpensesAsync(string userId, int? month, int? year, string? category, int pageNumber, int pageSize, bool showAll);
        Task<ExpenseCreateEditViewModel?> GetExpenseByIdAsync(int id, string userId);
        Task<bool> CreateExpenseAsync(ExpenseCreateEditViewModel viewModel, string userId);
        Task<bool> UpdateExpenseAsync(ExpenseCreateEditViewModel viewModel, string userId);
        Task<bool> DeleteExpenseAsync(int id, string userId);
    }
}
