using ExpenseTracker.Data;
using ExpenseTracker.Models.ViewModels.HomeViewModels;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<HomeIndexViewModel> GetHomeIndexDataAsync(string userId)
        {
            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var totalExpenses = await _context.Expenses
                .Where(e => e.UserId == userId && e.Date >= firstDayOfMonth && e.Date <= lastDayOfMonth)
                .SumAsync(e => e.Amount);

            var totalIncome = await _context.Incomes
                .Where(i => i.UserId == userId && i.Date >= firstDayOfMonth && i.Date <= lastDayOfMonth)
                .SumAsync(i => i.Amount);

            var remainingBalance = totalIncome - totalExpenses;

            var user = await _userManager.FindByIdAsync(userId);
            decimal allowedOverdraftLimit = user?.AllowedOverdraftLimit ?? 0.00m;

            var remainingOverdraftLimit = CalculateRemainingOverdraftLimit(remainingBalance, allowedOverdraftLimit);

            return new HomeIndexViewModel
            {
                ExpensesAmount = totalExpenses,
                IncomesAmount = totalIncome,
                TotalBalance = remainingBalance,
                AllowedOverdraftLimit = allowedOverdraftLimit,
                RemainingOverdraftLimit = remainingOverdraftLimit,
            };
        }

        public decimal CalculateRemainingOverdraftLimit(decimal balance, decimal allowedOverdraftLimit)
        {
            if (balance < 0)
                return allowedOverdraftLimit + balance;

            return allowedOverdraftLimit;
        }
    }
}
