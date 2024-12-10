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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HelperMethods _helperMethods;

        public IncomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HelperMethods helperMethods)
        {
            _context = context;
            _userManager = userManager;
            _helperMethods = helperMethods;
        }

        public async Task<IActionResult> Index(int? month, int? year, string? source, int pageNumber = 1, int pageSize = 10, bool showAll = false)
        {
            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;
            source ??= "all_sources";

            var userId = _userManager.GetUserId(User);

            var sourcesList = _helperMethods.GetSources();

            var hasSource = !string.IsNullOrEmpty(source) && source != "all_sources";

            var incomesQuery = _context.Incomes
                .Where(i => i.UserId == userId)
                .Where(i => i.Date.Month == month && i.Date.Year == year);

            if (hasSource)
            {
                incomesQuery = incomesQuery.Where(i => i.Source == source);
            }

            if (showAll)
            {
                var incomes = await incomesQuery.ToListAsync();

                var mappedIncomes = incomes.Select(i => new Income
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    Date = i.Date,
                    Description = i.Description,
                    Source = sourcesList.FirstOrDefault(s => s.Value == i.Source)?.Text ?? "Unknown Source"
                }).ToList();

                var viewModel = new IncomeIndexViewModel
                {
                    Incomes = mappedIncomes,
                    SelectedMonth = month,
                    SelectedYear = year,
                    SelectedSource = source,
                    ShowAll = true,
                    Months = _helperMethods.GetMonths(),
                    Sources = sourcesList,
                    PageSizeOptions = _helperMethods.pageSizeList,
                    PageSize = pageSize
                };

                return View(viewModel);
            }

            var paginatedIncomes = await PaginatedList<Income>.CreateAsync(incomesQuery, pageNumber, pageSize);

            var mappedPaginatedIncomes = paginatedIncomes.Items.Select(i => new Income
            {
                Id = i.Id,
                Amount = i.Amount,
                Date = i.Date,
                Description = i.Description,
                Source = sourcesList.FirstOrDefault(s => s.Value == i.Source)?.Text ?? "Unknown Source"
            }).ToList();

            var viewModelPaginated = new IncomeIndexViewModel
            {
                Incomes = mappedPaginatedIncomes,
                PaginatedItems = paginatedIncomes,
                SelectedMonth = month,
                SelectedYear = year,
                SelectedSource = source,
                ShowAll = false,
                Months = _helperMethods.GetMonths(),
                Sources = sourcesList,
                PageSizeOptions = _helperMethods.pageSizeList,
                PageSize = pageSize
            };

            return View(viewModelPaginated);
        }

        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            var income = _context.Incomes.FirstOrDefault(i => i.UserId == userId);

            var viewModel = new IncomeCreateViewModel
            {
                Amount = 0,
                Date = DateTime.Now,
                Source = string.Empty,
                Description = string.Empty,
                Sources = _helperMethods.GetSources()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncomeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Sources = _helperMethods.GetSources();
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

            return RedirectToAction("Index", "Income");
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
                Amount = income.Amount,
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
            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

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
