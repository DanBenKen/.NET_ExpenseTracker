using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
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
            var userId = _userManager.GetUserId(User);

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();

            var totalIncomeAmount = await _context.Incomes
                .Where(i => i.UserId == userId)
                .SumAsync(i => i.Amount) ?? 0m;
            var totalSpent = expenses.Sum(e => e.Amount);

            var remainingBalance = totalIncomeAmount - totalSpent;

            var user = await _userManager.FindByIdAsync(userId);

            decimal allowedOverdraftLimit = user.AllowedOverdraftLimit ?? 0.00m;

            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.UserId == userId);

            decimal remainingOverdraftLimit = user.AllowedOverdraftLimit.Value;

            if (remainingBalance < 0)
            {
                remainingOverdraftLimit = allowedOverdraftLimit + remainingBalance;
            }

            var viewModel = new HomeIndexViewModel
            {
                TotalBalance = remainingBalance,
                TotalSpent = totalSpent,
                AllowedOverdraftLimit = allowedOverdraftLimit,
                RemainingOverdraftLimit = remainingOverdraftLimit
            };

            return View(viewModel);
        }
    }
}
