using ExpenseTracker.Data;
using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interface;
using ExpenseTracker.Utils;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoryHelper _categoryHelper;
        private readonly MonthsHelper _monthsHelper;

        public ExpenseService(ApplicationDbContext context, CategoryHelper categoryHelper, MonthsHelper monthsHelper)
        {
            _context = context;
            _categoryHelper = categoryHelper;
            _monthsHelper = monthsHelper;
        }

        public async Task<ExpenseIndexViewModel> GetExpensesAsync(string userId, int? month, int? year, string? category, int pageNumber, int pageSize, bool showAll)
        {
            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;
            category ??= "all_categories";

            var categoriesList = _categoryHelper.GetCategories();
            bool hasValidCategory = !string.IsNullOrEmpty(category) && category != "all_categories";

            var expensesQuery = _context.Expenses
                .Where(e => e.UserId == userId)
                .Where(e => e.Date.Month == month && e.Date.Year == year);

            if (hasValidCategory)
            {
                expensesQuery = expensesQuery.Where(e => e.Category == category);
            }

            if (showAll)
            {
                var allExpenses = await expensesQuery.ToListAsync();

                var mappedExpenses = allExpenses.Select(e => new Expense
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    Date = e.Date,
                    Description = e.Description,
                    Category = categoriesList.FirstOrDefault(s => s.Value == e.Category)?.Text ?? "Unknown Source"
                }).ToList();

                return new ExpenseIndexViewModel
                {
                    Expenses = mappedExpenses,
                    SelectedMonth = month,
                    SelectedYear = year,
                    SelectedCategory = category,
                    ShowAll = true,
                    Months = _monthsHelper.GetMonths(),
                    Categories = categoriesList,
                    PageSize = pageSize
                };
            }

            var paginatedExpenses = await PaginatedList<Expense>.CreateAsync(expensesQuery, pageNumber, pageSize);

            var mappedPaginatedExpenses = paginatedExpenses.Items.Select(e => new Expense
            {
                Id = e.Id,
                Amount = e.Amount,
                Date = e.Date,
                Description = e.Description,
                Category = categoriesList.FirstOrDefault(s => s.Value == e.Category)?.Text ?? "Unknown Source"
            }).ToList();

            return new ExpenseIndexViewModel
            {
                Expenses = mappedPaginatedExpenses,
                PaginatedItems = paginatedExpenses,
                SelectedMonth = month,
                SelectedYear = year,
                SelectedCategory = category,
                ShowAll = false,
                Months = _monthsHelper.GetMonths(),
                Categories = categoriesList,
                PageSize = pageSize
            };
        }

        public async Task<bool> CreateExpenseAsync(ExpenseCreateEditViewModel viewModel, string userId)
        {
            var expense = new Expense
            {
                Description = viewModel.Description,
                Amount = viewModel.Amount,
                Date = viewModel.Date,
                Category = viewModel.Category,
                UserId = userId
            };

            _context.Add(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteExpenseAsync(int id, string userId)
        {
            var expense = await _context.Expenses.FindAsync(id);

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ExpenseCreateEditViewModel?> GetExpenseByIdAsync(int id, string userId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            return new ExpenseCreateEditViewModel
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Category = expense.Category,
                Date = expense.Date,
                Description = expense.Description,
                Categories = _categoryHelper.GetCategories()
            };
        }

        public async Task<bool> UpdateExpenseAsync(ExpenseCreateEditViewModel model, string userId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == model.Id && e.UserId == userId);

            if (expense == null)
            {
                return false;
            }

            expense.Date = model.Date;
            expense.Description = model.Description;
            expense.Category = model.Category;
            expense.Amount = model.Amount;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
