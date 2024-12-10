using ExpenseTracker.Data;
using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interface;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly HelperMethods _helperMethods;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExpenseService(ApplicationDbContext context, HelperMethods helperMethods, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _helperMethods = helperMethods;
            _userManager = userManager;
        }

        public async Task<ExpenseIndexViewModel> GetExpensesAsync(ClaimsPrincipal user, int? month, int? year, string? category, int pageNumber, int pageSize, bool showAll)
        {
            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;
            category ??= "all_categories";

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var categoriesList = _helperMethods.GetCategories();
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
                    Months = _helperMethods.GetMonths(),
                    Categories = categoriesList,
                    PageSizeOptions = _helperMethods.pageSizeList,
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
                Months = _helperMethods.GetMonths(),
                Categories = categoriesList,
                PageSizeOptions = _helperMethods.pageSizeList,
                PageSize = pageSize
            };
        }

        public async Task<bool> CreateExpenseAsync(ExpenseCreateEditViewModel viewModel, ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return false;
            }

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

        public async Task<bool> DeleteExpenseAsync(int id, ClaimsPrincipal user)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null || expense.UserId != user.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            {
                return false;
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ExpenseCreateEditViewModel?> GetExpenseByIdAsync(int id, ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
                return null;

            return new ExpenseCreateEditViewModel
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Category = expense.Category,
                Date = expense.Date,
                Description = expense.Description,
                Categories = _helperMethods.GetCategories()
            };
        }

        public async Task<bool> EditExpenseAsync(ExpenseCreateEditViewModel model, ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
