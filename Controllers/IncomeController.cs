using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.IncomeViewModels;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly HelperMethods _helperMethods;

        public IncomeController(ApplicationDbContext context, UserManager<User> userManager, HelperMethods helperMethods)
        {
            _context = context;
            _userManager = userManager;
            _helperMethods = helperMethods;
        }
        public async Task<IActionResult> Index(int? month, int? year, string? source)
        {
            var userId = _userManager.GetUserId(User);

            var incomesQuery = _context.Incomes.Where(i => i.UserId == userId);

            bool hasValidSource = !string.IsNullOrEmpty(source) && source != "AllSources";

            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;

            if (hasValidSource)
            {
                incomesQuery = incomesQuery.Where(e => e.Source == source);
            }

            incomesQuery = incomesQuery.Where(i => i.Date.Month == month && i.Date.Year == year);

            var incomes = await incomesQuery.ToListAsync();

            var viewModel = new IncomeViewModel
            {
                Incomes = incomes,
                SelectedMonth = month,
                SelectedYear = year,
                SelectedSource = source,
                Months = _helperMethods.GetMonths(),
                Sources = _helperMethods.GetSources(),
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            var income = _context.Incomes.FirstOrDefault(i => i.UserId == userId);

            var viewModel = new IncomeCreateViewModel
            {
                Amount = null,
                Date = DateTime.Now,
                Source = "",
                Description = "",
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncomeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userId = _userManager.GetUserId(User);

            var income = new Income
            {
                Amount = viewModel.Amount,
                Date = viewModel.Date,
                Source = viewModel.Source,
                Description = viewModel.Description,
                UserId = userId
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Expense");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (income == null)
            {
                return NotFound();
            }

            var viewModel = new IncomeCreateViewModel
            {
                Amount = income.Amount ?? 0m,
                Date = income.Date,
                Source = income.Source,
                Description = income.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IncomeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userId = _userManager.GetUserId(User);

            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            if (income == null)
            {
                return NotFound();
            }

            income.Amount = viewModel.Amount;
            income.Date = viewModel.Date;
            income.Source = viewModel.Source;
            income.Description = viewModel.Description;

            _context.Incomes.Update(income);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var income = await _context.Incomes
                                        .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (income == null)
            {
                return NotFound();
            }

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
