using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = _userManager.GetUserId(User);

            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var totalExpenses = await _context.Expenses
                .Where(e => e.UserId == userId && e.Date >= firstDayOfMonth && e.Date <= lastDayOfMonth)
                .SumAsync(e => e.Amount);

            var totalIncome = await _context.Incomes
                .Where(i => i.UserId == userId && i.Date >= firstDayOfMonth && i.Date <= lastDayOfMonth)
                .SumAsync(i => i.Amount);

            decimal remainingBalance = totalIncome - totalExpenses;

            var user = await _userManager.FindByIdAsync(userId);

            decimal allowedOverdraftLimit = user?.AllowedOverdraftLimit ?? 0.00m;
            var remainingOverdraftLimit = CalculateRemainingOverdraftLimit(remainingBalance, allowedOverdraftLimit);

            var viewModel = new HomeIndexViewModel
            {
                ExpensesAmount = totalExpenses,
                IncomesAmount = totalIncome,
                TotalBalance = remainingBalance,
                AllowedOverdraftLimit = allowedOverdraftLimit,
                RemainingOverdraftLimit = remainingOverdraftLimit,
            };

            return View(viewModel);
        }

        private decimal CalculateRemainingOverdraftLimit(decimal balance, decimal allowedOverdraftLimit)
        {
            if (balance < 0)
            {
                return allowedOverdraftLimit + balance;
            }

            return allowedOverdraftLimit;
        }
    }
}
